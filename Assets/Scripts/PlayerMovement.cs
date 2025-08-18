using UnityEngine;
using UnityEngine.InputSystem; // 한글 입력도 상호작용 가능하도록 
using UnityEngine.SceneManagement;

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


    void OnEnable()
    {
        // 씬이 로드될 때마다 OnSceneLoaded 함수를 호출하도록 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // 스크립트가 비활성화될 때 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    // 씬이 로드되었을 때 호출되는 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentInteractable = null;
    }

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); // 애니메이션 추가: 시작시 Animator 가져오기
    }

    void Update()
    {
        // (08/02 김지섭) 대화 중에는 플레이어 움직임 비활성화
        if (DialogueManager.instance != null && DialogueManager.instance.isDialogueActive)
        {
            // If dialogue is active, disable player movement
            movement = Vector2.zero;

            // (08/03 진성민)대화 중일 때도 애니메이션은 멈춤 상태가 되어야 하므로 speed를 0으로 설정
            if (anim != null)
            {
                anim.SetFloat("speed", 0);
            }
            return;
        }

        // Get input from WASD or arrow keys
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // (08/03 진성민) 애니메이터 로직 수정
        if (anim != null)
        {
            // speed 파라미터는 항상 현재 움직임의 크기를 반영
            anim.SetFloat("speed", movement.sqrMagnitude);

            // 캐릭터가 움직이고 있을 때만 (sqrMagnitude가 0보다 클 때만)
            // 걷기 방향(moveX, moveY)과 마지막 방향(lastMoveX, lastMoveY)을 업데이트
            if (movement.sqrMagnitude > 0.01f)
            {
                anim.SetFloat("moveX", movement.x);
                anim.SetFloat("moveY", movement.y);
                anim.SetFloat("lastMoveX", movement.x);
                anim.SetFloat("lastMoveY", movement.y);
            }
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

    //(08/03 진성민)
    // 다른 콜라이더의 트리거 범위에 들어갔을 때 한 번 호출
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 부딪힌 오브젝트에 Interactable 스크립트가 있는지 확인
        if (other.TryGetComponent<Interactable>(out Interactable newInteractable))
        {
            // 만약 기존에 다른 상호작용 오브젝트 범위 안에 있었다면, 그 오브젝트의 아이콘은 숨기기
            if (currentInteractable != null)
            {
                currentInteractable.HideIcons();
            }

            // 새로운 상호작용 대상을 설정하고, 아이콘을 표시하라고 직접 지시
            currentInteractable = newInteractable;
            currentInteractable.ShowIcons();
        }
    }

    // 다른 콜라이더의 트리거 범위에서 나갔을 때 한 번 호출
    private void OnTriggerExit2D(Collider2D other)
    {
        // 나간 오브젝트가 현재 상호작용 대상과 같다면,
        if (other.TryGetComponent<Interactable>(out Interactable exitedInteractable) && exitedInteractable == currentInteractable)
        {
            // 아이콘을 숨기고, 상호작용 대상 변수를 비운다
            currentInteractable.HideIcons();
            currentInteractable = null;
        }
    }
}
