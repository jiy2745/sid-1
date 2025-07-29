using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 movement;

    // (07/29 진성민) 새로 추가: 현재 상호작용 가능한 오브젝트를 저장할 변수
    private Interactable currentInteractable;

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

        // (07/29 진성민) 새로 추가: E키를 눌렀고, 상호작용할 오브젝트가 있다면
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            // 해당 오브젝트의 Interact() 함수를 호출
            currentInteractable.Interact();
        }
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


    // (07/29: 진성민) 새로 추가

    // 다른 콜라이더의 트리거 범위에 들어갔을 때 한 번 호출
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 부딪힌 오브젝트에 Interactable 스크립트가 있는지 확인
        if (other.TryGetComponent<Interactable>(out Interactable interactable))
        {
            // 있으면 현재 상호작용 대상으로 설정하기
            currentInteractable = interactable;
        }
    }

    // 다른 콜라이더의 트리거 범위에서 나갔을 때 한 번 호출
    private void OnTriggerExit2D(Collider2D other)
    {
        // 나간 오브젝트가 현재 상호작용 대상과 같다면, 대상을 null로 비우기
        if (other.TryGetComponent<Interactable>(out Interactable interactable) && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }
}