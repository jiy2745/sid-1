using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
    // (07/22: 진성민) 추가
    public bool stopPlayerOnEnter = true; 

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

            // (07/22: 진성민) 수정
            // playerMovement.OnMinigameStart();   // Stop player from moving: 기존코드
            if (stopPlayerOnEnter && playerMovement != null)
            {
                playerMovement.OnMinigameStart();
            }

            this.enabled = false;               // Disable this script to prevent duplicate events
        }
    }
}
