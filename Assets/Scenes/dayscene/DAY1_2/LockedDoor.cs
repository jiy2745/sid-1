using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    [Header("대화 관리자 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

   
    private bool playerIsInRange = false;

    void Start()
    {
        
        if (dialogueManager == null)
        {
            Debug.LogError("[LockedDoorController] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
    }

    void Update()
    {
        
        if (playerIsInRange && Input.GetKeyDown(KeyCode.E) && !dialogueManager.isDialogueActive)
        {
            ShowLockedMessage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 잠긴 문 영역에 들어옴.");
            playerIsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 잠긴 문 영역에서 벗어남.");
            playerIsInRange = false;

           
            if (dialogueManager != null && dialogueManager.isDialogueActive)
            {
                dialogueManager.ForceEndDialogue();
            }
        }
    }


    private void ShowLockedMessage()
    {
      
        Dialogue dialogue = new Dialogue();
        
      
        DialogueLine line = new DialogueLine
        {
            characterName = "나",
            dialogueText = "(옥상으로 향하는 길이다. 잠겨 있는 듯 하다)"
        };
        
       
        dialogue.dialogueLines.Add(line);
        
       
        dialogueManager.StartDialogue(dialogue, null);
    }
}