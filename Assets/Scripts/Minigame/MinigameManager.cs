using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Minigame
{
    NONE,
    QTE_MINIGAME,
    DODGE_MINIGAME,
    SLASH_MINIGAME
}

public class MinigameManager : MonoBehaviour
{
    public Minigame currentGame = Minigame.NONE;

    [Tooltip("Reference to player")]
    private GameObject player;

    [Tooltip("Reference to QTE Minigame prefab")]
    public GameObject qteMinigamePrefab;

    [Tooltip("Reference to Dodge Minigame prefab")]
    public GameObject dodgeMinigamePrefab;

    [Tooltip("Reference to Slash Minigame prefab")]
    public GameObject slashMinigamePrefab;

    public float minigameCooldown = 3f; // Cooldown between minigames, in seconds
    public List<int> minigameOrder = new List<int>
    {
        (int)Minigame.QTE_MINIGAME,
        (int)Minigame.DODGE_MINIGAME,
        (int)Minigame.SLASH_MINIGAME
    };
    private int currentMinigameIndex = 0;
    private HealthManager healthManager; // Reference to HealthManager for health management

    public UnityEvent onMinigamePhaseEnd; // Event to notify when a minigame phase ends

    // Reference to instance of each minigame prefab;
    private GameObject qteMinigameInstance;
    private GameObject dodgeMinigameInstance;
    private GameObject slashMinigameInstance;

    private float timer = 0f; // Timer for cooldown
    private bool isPaused = false; // Flag to check if minigame is paused

    void Awake()
    {
        healthManager = GetComponent<HealthManager>();
    }
        

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthManager.ShowHeartsUI(false); // Hide health UI at start
        currentGame = Minigame.NONE;
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not present in scene!");
        }
    }

    void Update()
    {
        // --- Minigame management ---
        if (currentGame == Minigame.NONE && !isPaused)
        {
            if (currentMinigameIndex >= minigameOrder.Count)
            {
                onMinigamePhaseEnd?.Invoke();
                this.enabled = false; // Disable this script if all minigames have been played
            }
            //TODO: Add logic to check if player had revive item
            // Play same minigame again if player has revive item

            timer += Time.deltaTime;
            if (timer >= minigameCooldown)
            {
                Debug.Log("Starting minigame: " + minigameOrder[currentMinigameIndex]);
                SetCurrentGame(minigameOrder[currentMinigameIndex++]);
                timer = 0f; // Reset timer after starting a minigame
            }
        }
    }

    // Used by events in minigames to inform they are finished, or in debugging-related UI
    public void SetCurrentGame(int id)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        switch (id)
        {
            // No minigame is running, enable player movement
            case (int)Minigame.NONE:
                // playerMovement.OnMinigameStop();

                currentGame = Minigame.NONE;
                healthManager.ShowHeartsUI(false); // Hide health UI when no minigame is active
                // destroy all minigames
                if (qteMinigameInstance != null) Destroy(qteMinigameInstance);
                if (dodgeMinigameInstance != null) Destroy(dodgeMinigameInstance);
                if (slashMinigameInstance != null) Destroy(slashMinigameInstance);
                break;

            case (int)Minigame.QTE_MINIGAME:
                currentGame = Minigame.QTE_MINIGAME;
                healthManager.ShowHeartsUI(true); // Show health UI
                healthManager.ResetHealth();
                if (qteMinigameInstance == null)
                {
                    qteMinigameInstance = Instantiate(qteMinigamePrefab, gameObject.transform);
                    //TODO: set transform.position of minigame (position of TriggerArea) accordingly
                    QTEMinigame game = qteMinigameInstance.GetComponent<QTEMinigame>();
                    // Subscribe to the new instance's game stop event
                    game.onMinigameStop.AddListener(() => SetCurrentGame(0));
                }
                // destroy other minigames
                if (dodgeMinigameInstance != null) Destroy(dodgeMinigameInstance);
                if (slashMinigameInstance != null) Destroy(slashMinigameInstance);
                break;

            case (int)Minigame.DODGE_MINIGAME:
                currentGame = Minigame.DODGE_MINIGAME;
                healthManager.ShowHeartsUI(true); // Show health UI
                healthManager.ResetHealth();
                if (dodgeMinigameInstance == null)
                {
                    dodgeMinigameInstance = Instantiate(dodgeMinigamePrefab, gameObject.transform);
                    //TODO: set transform.position of minigame (position of TriggerArea) accordingly
                    DodgeMinigame game = dodgeMinigameInstance.GetComponent<DodgeMinigame>();
                    // Subscribe to the new instance's game stop event
                    game.onMinigameStop.AddListener(() => SetCurrentGame(0));
                }
                // destroy other minigames
                if (qteMinigameInstance != null) Destroy(qteMinigameInstance);
                if (slashMinigameInstance != null) Destroy(slashMinigameInstance);
                break;

            case (int)Minigame.SLASH_MINIGAME:
                currentGame = Minigame.SLASH_MINIGAME;
                healthManager.ShowHeartsUI(true); // Show health UI
                healthManager.ResetHealth();
                if (slashMinigameInstance == null)
                {
                    slashMinigameInstance = Instantiate(slashMinigamePrefab, gameObject.transform);
                    SlashMinigame game = slashMinigameInstance.GetComponent<SlashMinigame>();
                    // Subscribe to the new instance's game stop event
                    game.onMinigameStop.AddListener(() => SetCurrentGame(0));
                }
                // destroy other minigames
                if (dodgeMinigameInstance != null) Destroy(dodgeMinigameInstance);
                if (qteMinigameInstance != null) Destroy(qteMinigameInstance);
                break;
        }
    }
}
