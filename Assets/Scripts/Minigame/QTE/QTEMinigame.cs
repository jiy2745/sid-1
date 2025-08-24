using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

// helper class
[System.Serializable]
public class KeyPrefab
{
    public KeyCode key;
    public GameObject prefab;
}
// helper class
public class ActiveKey
{
    public GameObject Instance { get; }
    public KeyCode Key { get; }

    public ActiveKey(GameObject instance, KeyCode key)
    {
        Instance = instance;
        Key = key;
    }
}

public class QTEMinigame : MonoBehaviour
{
    [Header("Minigame Settings")]
    [Tooltip("미니게임의 지속 시간 (초 단위)")]
    public float minigameDuration = 20.0f;

    [Header("Batch Settings")]
    [Tooltip("The number of keys that will be spawned in a single batch.")]
    public int keysPerBatch = 5;
    [Tooltip("The time in seconds between the end of one batch and the start of the next.")]
    public float batchSpawnInterval = 2.0f;

    public float timePerBatch = 5.0f;
    public float keySpawnDelay = 0.15f;

    [Tooltip("생성될 key prefab들의 리스트")]
    public KeyPrefab[] keyOptions;

    [Tooltip("Key가 생성되는 지점 (플레이어의 머리 위 등)")]
    public Transform keySpawnPoint;

    [Tooltip("스폰 지점에서부터 key가 생성될 수 있는 최대 거리")]
    public float spawnXRange = 5.0f;
    [Tooltip("플레이어의 위치에서 key가 생성되는 Y좌표가 떨어진 정도")]
    public float spawnYPosition = 3.0f;

    [Header("UI Elements")]
    public Image timerBar;
    public Gradient timerGradient;

    [Header("True로 설정 시, 화면에 없는 키를 누르면 패널티 부여")]
    public bool penalizeWrongKey = false;

    public UnityEvent onMinigameStop;

    private List<ActiveKey> currentBatch = new List<ActiveKey>();
    private float minigameTimer;    // Timer for whole minigame
    private float nextBatchTimer;       // Timer for new batch spawn
    private float batchTimer;           // Timer for current batch duration
    private bool gameActive = false;
    private bool isBatchActive = false;
    private HealthManager healthManager;
    [SerializeField] private Color spriteColor;

    [SerializeField] private GameObject EffectOnDestroyPrefab;      // Effect prefab for key prefab destruction

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        healthManager = GetComponentInParent<HealthManager>();
        keySpawnPoint = player.transform;
        if (timerBar != null)
        {
            timerBar.gameObject.SetActive(false);
        }
        StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameActive) return;

        // --- Minigame timer logic --- 
        minigameTimer -= Time.deltaTime;    // countdown timer
        if (minigameTimer <= 0)
        {
            StopMinigame(true);     // player won
            return;
        }

        // --- Key spawn logic ---
        if (isBatchActive)
        {
            // --- Key fall and input handling ---
            HandleBatchTimer();
            CheckPlayerInput();
        }
        else
        {
            // --- Inter-batch timer logic ---
            nextBatchTimer -= Time.deltaTime;
            if (nextBatchTimer <= 0)
            {
                StartCoroutine(SpawnBatchCoroutine());
            }
        }
    }

    private void HandleBatchTimer()
    {
         batchTimer -= Time.deltaTime;

        if (timerBar != null)
        {
            float fillAmount = Mathf.Clamp01(batchTimer / timePerBatch);
            timerBar.fillAmount = fillAmount;
            timerBar.color = timerGradient.Evaluate(1f - fillAmount); // Evaluate gradient based on time elapsed
        }

        if (batchTimer <= 0)
        {
            HandleFailure("Time ran out!");
        }
    }

    // handles player input
    private void CheckPlayerInput()
    {
        if (!Input.anyKeyDown) return;  // Early exit if no key was pressed
        bool correctKeyPressed = false;

        foreach (ActiveKey activeKey in new List<ActiveKey>(currentBatch))
        {
            KeyCode key = activeKey.Key;
            if (Input.GetKeyDown(key))
            {
                Debug.Log("pressed + " + key);
                HandleSuccess(activeKey);
                correctKeyPressed = true;
                break;
            }
        }

        // If a key was pressed but it didn't match any active key.
        if (!correctKeyPressed && penalizeWrongKey)
        {
            HandleFailure("Wrong key pressed");
        }
    }

    // called when player hits right key
    private void HandleSuccess(ActiveKey clearedKey)
    {
        if (EffectOnDestroyPrefab)
        {
            Instantiate(EffectOnDestroyPrefab, clearedKey.Instance.transform.position, Quaternion.identity);
        }

        Destroy(clearedKey.Instance);
        currentBatch.Remove(clearedKey);

        if (currentBatch.Count == 0)
        {
            EndBatch();
        }
 
    }

    // called when player misses a key, checks whether minigame over
    private void HandleFailure(string reason)
    {
        Debug.Log($"Batch failed : {reason}");
        // Wire to health system
        healthManager.DecreaseHealth();

        CleanUpAllKeys();

        // Game over check
        if (healthManager.currentHealth <= 0)
        {
            StopMinigame(false); // Player was defeated.
        }
        else
        {
            EndBatch();
        }
    }

    public void StartMinigame()
    {
        if (gameActive) return;
        if (!gameActive)
        {
            if (keySpawnPoint == null || keyOptions == null || keyOptions.Length == 0)
            {
                Debug.LogError("Configuration Error");
                return;
            }
            gameActive = true;
            isBatchActive = false;
            minigameTimer = minigameDuration;
            nextBatchTimer = 0;

            Debug.Log("Minigame start, active for " + minigameDuration + " seconds.");
            
            // Modify particle system color if applicable
            ParticleSystem ps = EffectOnDestroyPrefab.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.startColor = spriteColor;

                var col = ps.colorOverLifetime;
                col.enabled = true;

                // Gradient fades to transparent
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(spriteColor, 0.0f),
                    new GradientColorKey(spriteColor, 1.0f)
                },
                new GradientAlphaKey[] {
                new GradientAlphaKey(spriteColor.a, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f)
                }
                );
                col.color = new ParticleSystem.MinMaxGradient(gradient);
            }
        }
    }

    public void StopMinigame(bool playerWon)
    {
        if (!gameActive) return;

        gameActive = false;
        isBatchActive = false;
        StopAllCoroutines();
        CleanUpAllKeys();

        if (timerBar != null)
        {
            timerBar.gameObject.SetActive(false);
        }
        if (playerWon)
        {
            Debug.Log("Player Won");
            // Do additional stuff when won
        }
        else
        {
            Debug.Log("Player Lost");
            // Do additional stuff when lost
        }
        onMinigameStop?.Invoke();
    }

    private IEnumerator SpawnBatchCoroutine()
    {
        isBatchActive = true;
        batchTimer = timePerBatch;

        // Calculate the starting position to spawn keys from left to right.
        float startX = keySpawnPoint.position.x - spawnXRange / 2;
        float stepX = spawnXRange / (keysPerBatch - 1);

        for (int i = 0; i < keysPerBatch; i++)
        {
            KeyPrefab randomKeyPrefab = keyOptions[UnityEngine.Random.Range(0, keyOptions.Length)];

            // Position keys evenly across the spawn range.
            float xPos = startX + (i * stepX);
            Vector3 spawnPosition = new Vector3(xPos, keySpawnPoint.position.y + spawnYPosition, keySpawnPoint.position.z);

            GameObject newInstance = Instantiate(randomKeyPrefab.prefab, spawnPosition, keySpawnPoint.rotation);
            currentBatch.Add(new ActiveKey(newInstance, randomKeyPrefab.key));

            // Wait for the specified delay before spawning the next key.
            yield return new WaitForSeconds(keySpawnDelay);
        }
        // After spawning all keys, start the batch timer.
        if (timerBar != null)
        {
            timerBar.gameObject.SetActive(true);
        }
    }

    private void EndBatch()
    {
        isBatchActive = false;
        CleanUpAllKeys();
        nextBatchTimer = batchSpawnInterval;

        if (timerBar != null)
        {
            timerBar.gameObject.SetActive(false);
        }
    }

    // Destroys all prefab instances in activekeys list
    private void CleanUpAllKeys()
    {
        foreach (var key in currentBatch)
        {
            if (key.Instance != null)
            {
                Destroy(key.Instance);
            }
        }
        currentBatch.Clear();
    }
    
    // Removes any dangling references to the event
    private void OnDestroy()
    {
        onMinigameStop.RemoveAllListeners();
    }
}
