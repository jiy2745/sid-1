using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed = 5f; // Movement speed

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 movement;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void SetPlayerPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void OnMinigameStart()
    {
        // Hide sprite and disable this script
        this.enabled = false;
        //sprite.enabled = false;     
    }

    public void OnMinigameStop()
    {
        this.enabled = true;
        //sprite.enabled = true;
    }
}
