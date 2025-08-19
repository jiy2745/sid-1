using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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

    [Tooltip("생성될 key prefab들의 리스트")]
    public KeyPrefab[] keyOptions;

    [Tooltip("Key가 생성되는 지점 (플레이어의 머리 위 등)")]
    public Transform keySpawnPoint;

    [Tooltip("스폰 지점에서부터 key가 생성될 수 있는 최대 거리")]
    public float spawnXRange = 5.0f;
    [Tooltip("플레이어의 위치에서 key가 생성되는 Y좌표가 떨어진 정도")]
    public float spawnYPosition = 3.0f;

    [Header("Key Object Settings")]
    [Tooltip("Key의 추락 속도")]
    public float fallSpeed = 3.0f;
    [Tooltip("실패 판정이 되는 Y좌표 / key가 제거되는 좌표")]
    public float despawnYPosition = -5.0f;

    [Header("Player Settings (Map to player object later)")]
    public float playerHealth = 100.0f;
    public float healthPenalty = 20.0f;
    [Header("True로 설정 시, 화면에 없는 키를 누르면 패널티 부여")]
    public bool penalizeWrongKey = false;

    public UnityEvent onMinigameStop;

    private List<ActiveKey> currentBatch = new List<ActiveKey>();
    private float minigameTimer;    // Timer for whole minigame
    private float spawnTimer;       // Timer for each new key spawn
    private bool gameActive = false;
    private bool isWaitingForNextBatch = false;
    private HealthManager healthManager;
    [SerializeField] private Color spriteColor;

    [SerializeField] private GameObject EffectOnDestroyPrefab;      // Effect prefab for key prefab destruction

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        healthManager = GetComponentInParent<HealthManager>();
        keySpawnPoint = player.transform;
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
        if (isWaitingForNextBatch)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                SpawnNewKey();
            }
        }
        else
        {
             // --- Key fall and input handling ---
            HandleActiveKeys();
            CheckPlayerInput();
        }
    }

    // handles player input
    private void CheckPlayerInput()
    {
        bool correctKeyPressed = false;
        //ActiveKey keyToClear = null;

        foreach (ActiveKey activeKey in currentBatch)
        {
            KeyCode key = activeKey.Key;
            if (Input.GetKeyDown(key))
            {
                Debug.Log("pressed + " + key);
                HandleSuccess(activeKey);
                correctKeyPressed = true;
                /*
                // Find all keys that match the one pressed.
                var matchingKeys = activeKeys.Where(ak => ak.Key == key).ToList();
                if (matchingKeys.Any())
                {
                    // Order them by their Y position to get the one lowest on the screen.
                    keyToClear = matchingKeys.OrderBy(ak => ak.Instance.transform.position.y).First();
                    HandleSuccess(keyToClear);
                    correctKeyPressed = true;
                }
                */
                break;
            }
        }

        // If a key was pressed but it didn't match any active key.
        if (!correctKeyPressed && penalizeWrongKey)
        {
            HandleFailure(null);
        }
    }

    // Move key prefabs downwards and check for failure in each key
    private void HandleActiveKeys()
    {
        for (int i = currentBatch.Count - 1; i >= 0; i--)
        {
            ActiveKey key = currentBatch[i];
            // Move the key down in 2D space (Y-axis).
            key.Instance.transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);

            // Check if the key has fallen past the miss point.
            if (key.Instance.transform.position.y < despawnYPosition)
            {
                HandleFailure(key);
                return; // Exit after handling failure to avoid further processing.
            }
        }
    }

    // called when player hits right key
    private void HandleSuccess(ActiveKey clearedKey)
    {
        if (EffectOnDestroyPrefab)
        {
            Instantiate(EffectOnDestroyPrefab, clearedKey.Instance.transform.position, Quaternion.identity);
        }
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
        Destroy(clearedKey.Instance);
        currentBatch.Remove(clearedKey);

        if (currentBatch.Count == 0)
        {
            // If all keys in the current batch are cleared, prepare for the next batch.
            isWaitingForNextBatch = true;
            spawnTimer = batchSpawnInterval;
        }
 
    }

    // called when player misses a key, checks whether minigame over
    private void HandleFailure(ActiveKey key)
    {
        //playerHealth -= healthPenalty;
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
            isWaitingForNextBatch = true;
            spawnTimer = batchSpawnInterval;
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
            minigameTimer = minigameDuration;
            isWaitingForNextBatch = true;
            spawnTimer = 0;

            // TODO: temporary testing variable (remove and connect to actual player later!!!)
            playerHealth = 100.0f;
            Debug.Log("Minigame start, active for " + minigameDuration + " seconds.");
        }
    }

    public void StopMinigame(bool playerWon)
    {
        gameActive = false;
        CleanUpAllKeys();
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

    // spawns random key prefab at random location specified in keySpawnPoint
    private void SpawnNewKey()
    {
        isWaitingForNextBatch = false;

        for (int i = 0; i < keysPerBatch; i++)
        {
           KeyPrefab newKey = keyOptions[UnityEngine.Random.Range(0, keyOptions.Length)];
            float xPos = keySpawnPoint.position.x - (spawnXRange / 2) + (spawnXRange * (i + 0.5f) / keysPerBatch);
            Vector3 spawnPosition = new Vector3(keySpawnPoint.position.x + xPos, keySpawnPoint.position.y + spawnYPosition, keySpawnPoint.position.z);

            // instantiate new key prefab and store it to list
            GameObject newInstance = Instantiate(newKey.prefab, spawnPosition, keySpawnPoint.rotation);
            currentBatch.Add(new ActiveKey(newInstance, newKey.key));
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
