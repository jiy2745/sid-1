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
    [Tooltip("'그때였다' 대사 후 에리카가 이동할 위치 (Transform)")]
    public Transform ericaMove1;
    [Tooltip("모든 대사 종료 후 에리카가 이동할 위치 (Transform)")]
    public Transform ericaMove2;
    [Tooltip("에리카의 걷는 속도")]
    public float ericaSpeed = 2.5f;


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
        
        if (dialogueManager == null || ericaMove1 == null || ericaMove2 == null)
        {
            Debug.LogError("Dialogue Manager 또는 Erica 이동 위치(Transform)가 스크립트에 연결되지 않았습니다!");
            playerMovement.SetMovementActive(true);
            yield break;
        }

        GameObject ericaObject = GameObject.FindWithTag("Erica");
        if (ericaObject == null)
        {
            Debug.LogError("씬에서 'Erica' 태그를 가진 오브젝트를 찾을 수 없습니다!");
            playerMovement.SetMovementActive(true);
            yield break;
        }

        yield return new WaitForSeconds(0.2f);

       
        Dialogue dialoguePart1 = new Dialogue();
        dialoguePart1.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(괴물을 겨우 따돌렸지만 괴물은 다시 쫒아올 것 같다...)" });
        dialoguePart1.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그때였다)" });

        bool part1Complete = false;
        dialogueManager.StartDialogue(dialoguePart1, () => { part1Complete = true; }, true);

    
        yield return new WaitUntil(() => part1Complete);

    
        Debug.Log("에리카가 첫 번째 위치로 이동을 시작합니다.");
        yield return StartCoroutine(MoveCharacterCoroutine(ericaObject, ericaMove1.position, ericaSpeed));
        Debug.Log("에리카가 첫 번째 위치에 도착했습니다.");

    
        Dialogue dialoguePart2 = new Dialogue();
        dialoguePart2.dialogueLines.Add(new DialogueLine { characterName = "???", dialogueText = "도움이 필요해 보이네!" });
        dialoguePart2.dialogueLines.Add(new DialogueLine { characterName = "???", dialogueText = "몸은 좀 괜찮아?" });
        dialoguePart2.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 고개를 끄덕였다)" });
        dialoguePart2.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(성냥개비의 불빛 너머로 그녀의 얼굴을 자세히 보니, 학교를 돌아다니면서 가끔씩 봤던 얼굴이었다)" });
        dialoguePart2.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그리고 무엇보다 놀라운 건, 그녀의 눈은 2개였고, 그녀에게는 촉수가 달려있지 않았다)" });
        dialoguePart2.dialogueLines.Add(new DialogueLine { characterName = "???", dialogueText = "그 표정을 보니까 너도 깨달은 모양이구나?" });
        dialoguePart2.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "내 이름은 에리카야. 너처럼 이 세계의 끔찍한 진실을 알게 된 사람이지." });
        dialoguePart2.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "우리 복도에서 몇번 봤었지? 일단 더 자세한 이야기는 내일 하도록 하자." });
        dialoguePart2.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "여기 계속 있으면 위험할 테니까." });
        dialoguePart2.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "지금은 각자 기숙사로 돌아가는 게 좋겠어." });
        
        bool part2Complete = false;
        dialogueManager.StartDialogue(dialoguePart2, () => { part2Complete = true; }, true);
      
        yield return new WaitUntil(() => part2Complete);
        
      
        Debug.Log("에리카가 두 번째 위치로 이동을 시작합니다.");
        yield return StartCoroutine(MoveCharacterCoroutine(ericaObject, ericaMove2.position, ericaSpeed));
        Debug.Log("에리카가 두 번째 위치에 도착했습니다.");

       
        ericaObject.SetActive(false);
        Debug.Log("에리카 오브젝트를 비활성화합니다.");
      
        playerMovement.SetMovementActive(true);
        Debug.Log("대화 및 이벤트 종료. 플레이어 움직임을 활성화합니다.");
    }

    
    private IEnumerator MoveCharacterCoroutine(GameObject character, Vector3 targetPosition, float speed)
    {
        Animator animator = character.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError(character.name + "에서 Animator를 찾을 수 없습니다! 순간이동으로 대체합니다.");
            character.transform.position = targetPosition;
            yield break;
        }

        Vector2 lastDirection = Vector2.zero;

       
        while (Vector3.Distance(character.transform.position, targetPosition) > 0.1f)
        {
          
            Vector3 direction = (targetPosition - character.transform.position).normalized;
            lastDirection = new Vector2(direction.x, direction.y);

         
            character.transform.position = Vector3.MoveTowards(character.transform.position, targetPosition, speed * Time.deltaTime);

          
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            animator.SetFloat("speed", direction.sqrMagnitude); 

            yield return null; 
        }

      
        character.transform.position = targetPosition; 
        
      
        animator.SetFloat("speed", 0f);
        if(lastDirection != Vector2.zero)
        {
            animator.SetFloat("lastMoveX", lastDirection.x);
            animator.SetFloat("lastMoveY", lastDirection.y);
        }
    }
}