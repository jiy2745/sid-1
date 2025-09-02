using UnityEngine;
using System.Collections;

public class AfterMinigame2_3 : MonoBehaviour
{
    [Header("필수 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

    [Header("설정")]
    [Tooltip("미니게임 종료 후 플레이어가 이동할 위치")]
    public Vector3 destinationPosition;

    private bool hasTriggered = false; 

    void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true; 
            HandleAfterMinigame(collision.gameObject);
        }
    }

    private void HandleAfterMinigame(GameObject playerObject)
    {
       
        if (Camera.main != null)
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.enabled = true;
            }
        }

        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            playerMovement.SetPlayerPosition(destinationPosition);
            
        
            playerMovement.SetMovementActive(false);

            StartCoroutine(StartDialogueSequence(playerMovement));
        }
        else
        {
            Debug.LogError("PlayerMovement component not found on Player GameObject.");
        }
    }

    private IEnumerator StartDialogueSequence(PlayerMovement playerMovement)
    {
     
        if (dialogueManager == null)
        {
            Debug.LogError("Dialogue Manager가 AfterMinigame 스크립트에 연결되지 않았습니다!");
          
            playerMovement.SetMovementActive(true);
            yield break; 
        }
        
     
        yield return new WaitForSeconds(0.2f);

   
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(괴물을 겨우 따돌렸지만 괴물은 다시 쫒아올 것 같다...)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그때였다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "???", dialogueText = "도움이 필요해 보이네!" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "???", dialogueText = "몸은 좀 괜찮아?" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 고개를 끄덕였다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(성냥개비의 불빛 너머로 그녀의 얼굴을 자세히 보니, 학교를 돌아다니면서 가끔씩 봤던 얼굴이었다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그리고 무엇보다 놀라운 건, 그녀의 눈은 2개였고, 그녀에게는 촉수가 달려있지 않았다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "???", dialogueText = "그 표정을 보니까 너도 깨달은 모양이구나?" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "내 이름은 에리카야. 너처럼 이 세계의 끔찍한 진실을 알게 된 사람이지." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "우리 복도에서 몇번 봤었지? 일단 더 자세한 이야기는 내일 하도록 하자." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "여기 계속 있으면 위험할 테니까." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "지금은 각자 기숙사로 돌아가는 게 좋겠어." });

      
        System.Action onDialogueComplete = () => {
            if (playerMovement != null)
            {
                playerMovement.SetMovementActive(true);
                Debug.Log("대화 종료. 플레이어 움직임을 활성화합니다.");
            }
        };


        dialogueManager.StartDialogue(dialogue, onDialogueComplete, true);
    }
}