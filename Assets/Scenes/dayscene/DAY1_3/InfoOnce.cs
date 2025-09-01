using UnityEngine;

public class RoomEntryEvent : MonoBehaviour
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

        // 1. 요청하신 대사를 순서대로 추가합니다.
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(기숙사 안의 내 방으로 왔다.)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "고양이", dialogueText = "야옹." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(방 안에 고양이가 들어와 있다. 창문을 통해 들어온 것 같다)" });

        dialogueManager.StartDialogue(dialogue, OnEventComplete, true);
    }

    private void OnEventComplete()
    {
        Debug.Log("방 입장 이벤트 완료. 이 트리거를 비활성화합니다.");
   
        gameObject.SetActive(false);
    }
}