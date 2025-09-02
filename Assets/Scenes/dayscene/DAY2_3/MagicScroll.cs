using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDay2Interaction : MonoBehaviour
{
    [Header("필수 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

    private bool playerIsInRange = false;
    private bool eventTriggered = false; 
    private GameObject playerObject;     

    void Update()
    {
       
        if (playerIsInRange && Input.GetKeyDown(KeyCode.E) && !eventTriggered && !dialogueManager.isDialogueActive)
        {
            StartBedSequence();
        }
    }

    private void StartBedSequence()
    {
        eventTriggered = true; 

      
        if (playerObject != null)
        {
            PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetMovementActive(false);
                Debug.Log("플레이어 움직임을 비활성화합니다.");
            }
        }

       
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(침대에 낡은 종이가 놓여져 있다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그 종이에는 이렇게 적혀 있었다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "'소환 마법 스크롤'" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "'이 종이를 찢어 단 한 번, 결정적인 순간 깡총거리는 짐승들에게 도움을 요청할 수 있다'" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "'그러나 그 짐승들에게 충분한 호의를 얻지 못했다면, 그들은 응답하지 않을 것이다' 라고 적혀 있다'" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(원래 같았으면 이런 허무맹랑한 소리가 적혀 있는 종이는 그냥 무시했을 것이다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그러나 지금은 이미 상식을 벗어난 상황이었기에 일단 이 종이를 챙겨두기로 했다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그 후 나는 잠에 들었다)" });

        
        dialogueManager.StartDialogue(dialogue, OnDialogueComplete, true);
    }

    private void OnDialogueComplete()
    {
        Debug.Log("대화 종료. Day3으로 전환합니다.");

   
        if (GameManager.instance != null)
        {
            GameManager.instance.nextPlayerSpawnPointName = "from2_3dorm";
          
        }
        else
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다! 스폰 위치를 지정할 수 없습니다.");
        }

     
        SceneManager.LoadScene("Day3_classroom");
    }

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInRange = true;
            playerObject = other.gameObject; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInRange = false;
            playerObject = null; 
        }
    }
}
