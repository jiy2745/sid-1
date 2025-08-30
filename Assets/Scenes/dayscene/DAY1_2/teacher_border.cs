using UnityEngine;

public class TeacherController : MonoBehaviour
{
    [Header("오브젝트 연결")]
    [Tooltip("실제 선생님의 스프라이트나 모델이 있는 게임 오브젝트")]
    public GameObject teacherObject;
    
    [Header("대화 관리자 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

    private Animator anim;
    
    private bool playerIsInRange = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        
        if (dialogueManager == null)
        {
            Debug.LogError("[TeacherController] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
        
        if (GameManager.instance != null)
        {
            GameManager.instance.onStateChanged.AddListener(CheckActionsLeft);
            CheckActionsLeft(); 
        }
        else
        {
            Debug.LogError("GameManager를 찾을 수 없음. 씬에 GameManager가 있는지 확인 필요");
        }
    }

    void Update()
    {
        if (playerIsInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueManager != null && !dialogueManager.isDialogueActive)
            {
                ShowBlockingDialogue();
            }
        }
    }

    private void CheckActionsLeft()
    {
        if (teacherObject == null)
        {
            Debug.LogError("[TeacherController] Teacher Object 변수가 빔. Inspector에서 설정 필요.");
            return;
        }

        bool isActive = GameManager.instance.actionsLeft > 0;
        teacherObject.SetActive(isActive);

        if(isActive)
        {
            Debug.Log($"[TeacherController] 행동 횟수가 {GameManager.instance.actionsLeft}이므로 선생님 오브젝트를 활성화");
        }
        else
        {
            Debug.Log($"[TeacherController] 행동 횟수가 0 이하이므로 선생님 오브젝트를 비활성화");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지! 상호작용 가능 상태로 변경");
            playerIsInRange = true;
            if (anim != null)
            {
                anim.SetBool("PlayerIsNear", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 벗어남, 상호작용 불가능 상태로 돌아감");
            playerIsInRange = false;
            if (anim != null)
            {
                anim.SetBool("PlayerIsNear", false);
            }

           
            if (dialogueManager != null && dialogueManager.isDialogueActive)
            {
                dialogueManager.ForceEndDialogue();
            }
        }
    }
    
    private void ShowBlockingDialogue()
    {
        Dialogue dialogue = new Dialogue();
        
        DialogueLine line = new DialogueLine
        {
            characterName = "선생님",
            dialogueText = "주번 일을 다 하지 않고 도망가려는 거니?"
        };
        
        dialogue.dialogueLines.Add(line);
        
        dialogueManager.StartDialogue(dialogue, null); 
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onStateChanged.RemoveListener(CheckActionsLeft);
        }
    }
}