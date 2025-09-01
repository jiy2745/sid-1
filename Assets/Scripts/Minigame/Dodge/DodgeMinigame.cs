using UnityEngine;
using UnityEngine.Events;

public class DodgeMinigame : MonoBehaviour
{
    [Header("Minigame Settings")]
    [Tooltip("미니게임의 지속 시간 (초 단위)")]
    public float minigameDuration = 20.0f;
    public DodgeMinigameSpawner spawner;
    public UnityEvent onMinigameStop;

    private float minigameTimer;    // Timer for whole minigame
    private bool gameActive = false;
    private HealthManager healthManager; // Reference to the HealthManager component
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthManager = GetComponentInParent<HealthManager>();
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
    }
    public void StartMinigame()
    {
        if (gameActive) return;
        else
        {
            if (spawner == null)
            {
                Debug.LogError("Configuration Error");
                return;
            }
            gameActive = true;
            minigameTimer = minigameDuration;
            StartCoroutine(spawner.SpawnDodgeObject());

            Debug.Log("Minigame start, active for " + minigameDuration + " seconds.");
        }
    }
    public void StopMinigame(bool playerWon)
    {
        gameActive = false;
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

    public void DecreaseHealth()
    {
        if (healthManager != null)
        {
            healthManager.DecreaseHealth();
        }
        else
        {
            Debug.LogError("HealthManager not found in parent!");
        }
    }
}
