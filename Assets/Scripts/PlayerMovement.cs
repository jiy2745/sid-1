using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 movement;
    private Animator anim;
    private Interactable currentInteractable;
    
 
    private bool canMove = true;


    void OnEnable()
    {
       
        SceneManager.sceneLoaded += OnSceneLoaded;
        
       
        day1_dialogmanager.OnDialogueStart += DisableMovement;
        day1_dialogmanager.OnDialogueEnd += EnableMovement;
    }

    void OnDisable()
    {
       
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        
        day1_dialogmanager.OnDialogueStart -= DisableMovement;
        day1_dialogmanager.OnDialogueEnd -= EnableMovement;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentInteractable = null;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       
        if (canMove)
        {
           
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            
            movement = Vector2.zero;
        }
        
      
        if (anim != null)
        {
            anim.SetFloat("speed", movement.sqrMagnitude);

            if (movement.sqrMagnitude > 0.01f)
            {
                anim.SetFloat("moveX", movement.x);
                anim.SetFloat("moveY", movement.y);
                anim.SetFloat("lastMoveX", movement.x);
                anim.SetFloat("lastMoveY", movement.y);
            }
        }

    
        if (Keyboard.current.eKey.wasPressedThisFrame && currentInteractable != null && canMove)
        {
            currentInteractable.Interact();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

   
    private void DisableMovement()
    {
        canMove = false;
        Debug.Log("대화 시작: 플레이어 움직임 비활성화");
    }

  
    private void EnableMovement()
    {
        canMove = true;
        Debug.Log("대화 종료: 플레이어 움직임 활성화");
    }



    public void SetPlayerPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void OnMinigameStart()
    {
        this.enabled = false;
    }

    public void OnMinigameStop()
    {
        this.enabled = true;
    }

    public void SetMovementActive(bool isActive)
    {
        if (!isActive)
        {
            rb.velocity = Vector2.zero;
            if (anim != null)
            {
                anim.SetFloat("speed", 0);
            }
        }
        this.enabled = isActive;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable newInteractable))
        {
            if (currentInteractable != null)
            {
                currentInteractable.HideIcons();
            }
            currentInteractable = newInteractable;
            currentInteractable.ShowIcons();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable exitedInteractable) && exitedInteractable == currentInteractable)
        {
            currentInteractable.HideIcons();
            currentInteractable = null;
        }
    }
}