using UnityEngine;
using UnityEngine.InputSystem; // 한글 입력도 상호작용 가능하도록 

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 movement;

    // (07/31 진성민) : 애니메이터를 담을 변수
    private Animator anim;

    // (07/29 진성민) 새로 추가: 현재 상호작용 가능한 오브젝트를 저장할 변수
    private Interactable currentInteractable;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); // 애니메이션 추가: 시작시 Animator 가져오기
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        //애니메이션 추가: Animator 파라미터 값 설정
        if (anim != null)
        {
            anim.SetFloat("moveX", movement.x);
            anim.SetFloat("moveY", movement.y);
            anim.SetFloat("speed", movement.sqrMagnitude);
        }

        // 애니메이션 추가: 캐릭터 좌우 반전 로직
        if (movement.x > 0)
        {
            sprite.flipX = true; // 오른쪽을 볼 때 (원본)
        }
        else if (movement.x < 0)
        {
            sprite.flipX = false; // 왼쪽을 볼 때 (좌우 반전)
        }

        // (07/29 진성민) 새로 추가: E키를 눌렀고, 상호작용할 오브젝트가 있다면
        if (Keyboard.current.eKey.wasPressedThisFrame && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        // 애니메이션시 대각선 이동 속도 빨라짐 방지
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
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

            // 움직임이 비활성화되면 애니메이터도 정지. 속도0
             if (anim != null)
            {
                anim.SetFloat("speed", 0);
            }
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