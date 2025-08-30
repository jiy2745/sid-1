using UnityEngine;

// 이 스크립트는 Collider2D가 있는 오브젝트에 붙여야 합니다.
[RequireComponent(typeof(Collider2D))]
public class NPCInteraction : MonoBehaviour
{
    [Header("대화 설정")]
    [Tooltip("씬에 있는 대화창 관리자를 연결해주세요.")]
    [SerializeField] private day1_dialogmanager dialogueManager;
    
    [Tooltip("이 NPC의 이름을 입력하세요.")]
    [SerializeField] private string characterName = "선생님";

    [Tooltip("플레이어가 상호작용 시 출력할 대사입니다.")]
    [TextArea(3, 5)] 
    [SerializeField] private string dialogueText = "주번 일을 다 하지 않고 도망가려는 거니?";
    
 
    private Animator anim;

    void Start()
    {
        
        anim = GetComponentInChildren<Animator>();

        
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<day1_dialogmanager>();
            if (dialogueManager == null)
            {
                Debug.LogError("씬에서 'day1_dialogmanager'를 찾을 수 없습니다!", this.gameObject);
            }
        }
    }

 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            if (anim != null)
            {
                anim.SetBool("PlayerIsNear", true);
            }
        }
    }

    
    private void OnTriggerStay2D(Collider2D other)
    {
       
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
          
            if (dialogueManager != null && !dialogueManager.isDialogueActive)
            {
                ShowDialogue();
            }
        }
    }

    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (anim != null)
            {
                anim.SetBool("PlayerIsNear", false);
            }
        }
    }

 
    private void ShowDialogue()
    {
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine 
        { 
            characterName = this.characterName, 
            dialogueText = this.dialogueText 
        });
        
        dialogueManager.StartDialogue(dialogue, null);
    }
}