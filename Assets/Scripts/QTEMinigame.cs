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

    [Tooltip("생성될 key prefab들의 리스트")]
    public KeyPrefab[] keyOptions;

    [Tooltip("Key가 생성되는 지점 (플레이어의 머리 위 등)")]
    public Transform keySpawnPoint;

    [Tooltip("스폰 지점에서부터 key가 생성될 수 있는 최대 거리")]
    public float spawnXRange = 5.0f;

    [Tooltip("새 key의 생성 주기")]
    public float spawnInterval = 1.0f;

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

    private List<ActiveKey> activeKeys = new List<ActiveKey>();
    private float minigameTimer;    // timer for whole minigame
    private float spawnTimer;       // timer for each new key spawn
    private bool gameActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.enabled = false;   // Disable script at start (managed by minigame manager)
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
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnNewKey();
            spawnTimer = spawnInterval;
        }

        // --- Key fall and input handling ---
        HandleActiveKeys();
        CheckPlayerInput();
    }

     // handles player input
    private void CheckPlayerInput()
    {
        bool correctKeyPressed = false;
        ActiveKey keyToClear = null;

        foreach (ActiveKey activeKey in activeKeys)
        {
            KeyCode key = activeKey.Key;
            if (Input.GetKeyDown(key))
            {
                Debug.Log("pressed + " + key);
                // Find all keys that match the one pressed.
                var matchingKeys = activeKeys.Where(ak => ak.Key == key).ToList();
                if (matchingKeys.Any())
                {
                    // Order them by their Y position to get the one lowest on the screen.
                    keyToClear = matchingKeys.OrderBy(ak => ak.Instance.transform.position.y).First();
                    HandleSuccess(keyToClear);
                    correctKeyPressed = true;
                }
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
        for (int i = activeKeys.Count - 1; i >= 0; i--)
        {
            ActiveKey key = activeKeys[i];
            // Move the key down in 2D space (Y-axis).
            key.Instance.transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);

            // Check if the key has fallen past the miss point.
            if (key.Instance.transform.position.y < despawnYPosition)
            {
                HandleFailure(key);
            }
        }
    }

    // called when player hits right key
    private void HandleSuccess(ActiveKey clearedKey)
    {
        Destroy(clearedKey.Instance);
        activeKeys.Remove(clearedKey);
    }

    // called when player misses a key, checks whether minigame over
    private void HandleFailure(ActiveKey key)
    {
        playerHealth -= healthPenalty;

        // Remove missed key
        if (key != null)
        {
            Destroy(key.Instance);
            activeKeys.Remove(key);
        }
        // Game over check
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            StopMinigame(false); // Player was defeated.
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
            // Do additional stuff
        }
        else
        {
            Debug.Log("Player Lost");
            // Do additional stuff
        }
        onMinigameStop?.Invoke();
        this.enabled = false;   // stop this script from running
    }

    // spawns random key prefab at random location specified in keySpawnPoint
    private void SpawnNewKey()
    {
        KeyPrefab newKey = keyOptions[UnityEngine.Random.Range(0, keyOptions.Length)];
        float XPos = UnityEngine.Random.Range(-spawnXRange / 2, spawnXRange / 2);
        Vector3 spawnPosition = new Vector3(keySpawnPoint.position.x + XPos, keySpawnPoint.position.y, keySpawnPoint.position.z);

        // instantiate new key prefab and store it to list
        GameObject newInstance = Instantiate(newKey.prefab, spawnPosition, keySpawnPoint.rotation);
        activeKeys.Add(new ActiveKey(newInstance, newKey.key));
    }

    // Destroys all prefab instances in activekeys list
    private void CleanUpAllKeys()
    {
        foreach (var key in activeKeys)
        {
            if (key.Instance != null)
            {
                Destroy(key.Instance);
            }
        }
        activeKeys.Clear();
    }
}
