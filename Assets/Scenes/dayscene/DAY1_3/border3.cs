using UnityEngine;

public class border3 : MonoBehaviour
{
    [Header("대화 관리자 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

    

    void Start()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("[border3] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !dialogueManager.isDialogueActive)
        {
            ShowNightMessage();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogueManager != null && dialogueManager.isDialogueActive)
            {
                dialogueManager.ForceEndDialogue();
            }
        }
    }

    private void ShowNightMessage()
    {
        Dialogue dialogue = new Dialogue();
        
        DialogueLine line = new DialogueLine
        {
            characterName = "나",
            dialogueText = "(어두워졌어... 이제 기숙사로 가야해)"
        };
        
        dialogue.dialogueLines.Add(line);
        
        dialogueManager.StartDialogue(dialogue, null);
    }
}