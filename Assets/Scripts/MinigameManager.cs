using UnityEngine;
using UnityEngine.Events;

public enum Minigame
{
    NONE,
    QTE_MINIGAME,
    DROP_MINIGAME,
    SLASH_MINIGAME
}

public class MinigameManager : MonoBehaviour
{
    public Minigame currentGame = Minigame.NONE;

    [Tooltip("Reference to player")]
    public GameObject player;

    [Tooltip("Reference to QTE Minigame prefab")]
    public GameObject qteMinigamePrefab;

    private GameObject qteMinigameInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGame = Minigame.NONE;

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
                if (qteMinigameInstance != null) Destroy(qteMinigameInstance);
                // disable other minigames
                break;

            case (int)Minigame.QTE_MINIGAME:
                currentGame = Minigame.QTE_MINIGAME;
                if (qteMinigameInstance == null)
                {
                    qteMinigameInstance = Instantiate(qteMinigamePrefab);
                    QTEMinigame game = qteMinigameInstance.GetComponent<QTEMinigame>();
                    // Subscribe to the new instance's game stop event
                    game.onMinigameStop.AddListener(() => SetCurrentGame(0));
                } 
                // disable other minigames
                break;
        }
    }
}
