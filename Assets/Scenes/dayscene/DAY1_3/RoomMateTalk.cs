using UnityEngine;

public class RoomMateTalk : MonoBehaviour
{
    [Header("대화 관리자 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

  
    private bool playerIsInRange = false;
    
    private bool hasInteracted = false;

    void Start()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("[SleepingRoommate] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
    }

    void Update()
    {
      
        if (playerIsInRange && Input.GetKeyDown(KeyCode.E) && !dialogueManager.isDialogueActive && !hasInteracted)
        {
            ShowSleepingMessage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInRange = false;
        }
    }

    private void ShowSleepingMessage()
    {
    
        hasInteracted = true;
        
        Debug.Log("룸메이트가 자고 있는 것을 확인합니다.");
        Dialogue dialogue = new Dialogue();

        DialogueLine line = new DialogueLine
        {
            characterName = "나",
            dialogueText = "(룸메이트는 자고 있는 것 같다)"
        };
        
        dialogue.dialogueLines.Add(line);
        
    
        dialogueManager.StartDialogue(dialogue, null, true);
    }
}