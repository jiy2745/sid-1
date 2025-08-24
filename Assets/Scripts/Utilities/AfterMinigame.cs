using UnityEngine;

public class AfterMinigame : MonoBehaviour
{
    [Tooltip("Destination position to move the player after the minigame ends")]
    public Vector3 destinationPosition;
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.CompareTag("Player"))
        {
            // Call the method to handle after minigame logic
            HandleAfterMinigame();
        }
    }

    private void HandleAfterMinigame()
    {
        // Set camera follow script to follow the player
        Camera.main.GetComponent<CameraFollow>().enabled = true;

        // Get the PlayerMovement component from the player GameObject
        PlayerMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            // Set the player's position to the destination position
            playerMovement.SetPlayerPosition(destinationPosition);
            playerMovement.SetMovementActive(true);
        }
        else
        {
            Debug.LogError("PlayerMovement component not found on Player GameObject.");
        }
    }
}

// TODO: Add logic to check the current day, for specific events
// Right now, we only handle the standard case excluding day 1 and final day