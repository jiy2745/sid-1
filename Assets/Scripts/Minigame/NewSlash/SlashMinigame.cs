using System;
using UnityEngine;
using UnityEngine.Events;

public class SlashMinigame : MonoBehaviour
{
    [Header("References")]
    public SlashBar slashBar; // Reference to the SlashBar component
    public Animator playerAnimator; // Reference to the player's animator
    public UnityEngine.UI.Image radialTimerImage; // Reference to the radial timer image
    public Vector3 radialTimerOffset = new Vector3(100, 70, 0); // Offset for the radial timer position
    public Vector3 slashBarOffset = new Vector3(0, -200, 0); // Offset for the slash bar position
    public UnityEvent onMinigameStop;

    [Header("Settings")]
    public int totalSegments = 5;
    public int lives = 3; // Number of lives the player has
    public float segmentTimelimit = 3f; // Time allowed for each segment

    private int currentSegment = 0; // Current segment the player is on
    private float minigameTimer;    // Timer for whole minigame
    private bool gameActive = false;

    private Camera mainCamera;
    private Transform playerTransform;
    private RectTransform slashBarTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
        GameObject player = GameObject.FindWithTag("Player");   // Get the player GameObject
        if (player == null)
        {
            Debug.LogError("Player GameObject not found.");
            return;
        }
        playerAnimator = player.GetComponent<Animator>();
        playerTransform = player.transform;
        slashBarTransform = slashBar.GetComponent<RectTransform>();
        StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameActive) return;

        // --- Minigame timer logic --- 
        minigameTimer -= Time.deltaTime;    // countdown timer
        radialTimerImage.fillAmount = minigameTimer / segmentTimelimit; // Update the radial timer image
        if (minigameTimer <= 0)
        {
            SegmentFailed();
        }

        // --- Object positioning ---
        Vector3 screenPos = mainCamera.WorldToScreenPoint(playerTransform.position);
        radialTimerImage.transform.position = screenPos + radialTimerOffset; // Position the radial timer image above the player
        slashBarTransform.position = screenPos + slashBarOffset; // Position the slash bar below the player

        // --- Input handling ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Slash"); // Trigger the slash animation
            }
            bool success = slashBar.CheckIfInSafeZone(); // Check if the player slashed in the safe zone
            if (success)
            {
                currentSegment++;
                if (currentSegment >= totalSegments)
                {
                    StopMinigame(true); // Player won the minigame
                }
                else
                {
                    StartNewSegment(); // Start the next segment
                }
            }
            else
            {
                SegmentFailed(); // Player failed the segment
            }
        }
    }

    private void SegmentFailed()
    {
        lives--;
        if (lives <= 0)
        {
            StopMinigame(false); // Player lost the minigame
        }
        else
        {
            Debug.Log("Segment failed, lives left: " + lives);
            StartNewSegment(); // Start a new segment
        }
    }

    public void StartMinigame()
    {
        if (gameActive) return;
        else
        {
            gameActive = true;
            StartNewSegment();
            Debug.Log("Minigame start");
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
    
       private void StartNewSegment()
    {
        int difficulty = UnityEngine.Random.Range(0, 3); // Randomly select a difficulty level
        slashBar.GenerateSafeZone(difficulty);

        // Reset the radial timer
        minigameTimer = segmentTimelimit;
        radialTimerImage.fillAmount = 1f; // Reset the radial timer image
    }
}
