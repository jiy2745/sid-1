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

    // Reference to instance of each minigame prefab;
    private GameObject qteMinigameInstance;
    private GameObject dodgeMinigameInstance;
    private GameObject slashMinigameInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGame = Minigame.NONE;
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not present in scene!");
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
                playerMovement.OnMinigameStop();

                currentGame = Minigame.NONE;
                // destroy all minigames
                if (qteMinigameInstance != null) Destroy(qteMinigameInstance);
                if (dodgeMinigameInstance != null) Destroy(dodgeMinigameInstance);
                if (slashMinigameInstance != null) Destroy(slashMinigameInstance);
                break;

            case (int)Minigame.QTE_MINIGAME:
                currentGame = Minigame.QTE_MINIGAME;
                if (qteMinigameInstance == null)
                {
                    qteMinigameInstance = Instantiate(qteMinigamePrefab);
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
                break;
            case (int)Minigame.SLASH_MINIGAME:
                currentGame = Minigame.SLASH_MINIGAME;
                break;
        }
    }
}
