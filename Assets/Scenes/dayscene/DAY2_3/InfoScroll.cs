using UnityEngine;

public class InfoScroll : MonoBehaviour
{
    [Header("필수 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

   
    private bool eventTriggered = false;

    void Start()
    {
      
        if (dialogueManager == null)
        {
            Debug.LogError("[RoomEntryEvent] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !eventTriggered && !dialogueManager.isDialogueActive)
        {
            
            eventTriggered = true;
            StartRoomEntrySequence();
        }
    }

    private void StartRoomEntrySequence()
    {
        Debug.Log("방 입장 이벤트 시작.");
        Dialogue dialogue = new Dialogue();

      
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(침대 위에 이상한 종이가 놓여있다)" });
        

        dialogueManager.StartDialogue(dialogue, OnEventComplete, true);
    }

    private void OnEventComplete()
    {
        Debug.Log("방 입장 이벤트 완료. 이 트리거를 비활성화합니다.");
   
        gameObject.SetActive(false);
    }
}