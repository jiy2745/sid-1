using UnityEngine;

public class EricaTalk : MonoBehaviour
{
    [Header("대화 관리자 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

    // 에리카의 애니메이터 컴포넌트
    private Animator anim;
    
   
    private bool playerIsInRange = false;

    void Start()
    {
        
        anim = GetComponentInChildren<Animator>();
        
        if (dialogueManager == null)
        {
            Debug.LogError("[EricaTalk] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
    }

    void Update()
    {
     
        if (playerIsInRange && Input.GetKeyDown(KeyCode.E))
        {
            
            if (dialogueManager != null && !dialogueManager.isDialogueActive)
            {
                StartEricaDialogue();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지! 에리카가 쳐다봅니다.");
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
            Debug.Log("플레이어가 벗어남, 에리카가 원래 상태로 돌아갑니다.");
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
    
 
    private void StartEricaDialogue()
    {
      
        Dialogue dialogue = new Dialogue();
        
        
        dialogue.dialogueLines.Add(new DialogueLine
        {
            characterName = "?",
            dialogueText = "..."
        });
        
        

        dialogue.dialogueLines.Add(new DialogueLine
        {
            characterName = "나",
            dialogueText = "(그녀를 보니 무언가 이상한 기분이 들었다)"
        });
        
       
        dialogueManager.StartDialogue(dialogue, null); 
    }
}