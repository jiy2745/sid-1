using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
    public UnityEvent onPlayerEnter;
    private GameObject player;
    private PlayerMovement playerMovement;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onPlayerEnter?.Invoke();
            playerMovement.OnMinigameStart();   // Stop player from moving
            this.enabled = false;               // Disable this script to prevent duplicate events
        }
    }
}
