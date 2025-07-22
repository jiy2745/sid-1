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

    

    // (07/22: 진성민) 이 아래 부분을 추가했습니다! 
    // PlayerMovement 스크립트를 켜거나 끄는 기능.
    // "isActive" 가 true면 켜고, false면 끈다. 

    public void SetMovementActive(bool isActive)
    {
        if (!isActive)
        {
            // 움직임을 멈추기 위해 물리적 속도를 0으로 만들기. 
            rb.linearVelocity = Vector2.zero;
        }
        // 해당 스크립트 자체를 켜거나 끔.
        this.enabled = isActive;
    }


}
