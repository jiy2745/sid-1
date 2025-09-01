using UnityEngine;
using System.Collections;

public class catTalk : MonoBehaviour
{
    [Header("필수 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;
    [Tooltip("대화가 끝난 후 고양이가 이동할 지점(빈 오브젝트)을 연결해주세요.")]
    public Transform catMovePoint;

    [Header("설정")]
    [Tooltip("고양이의 이동 속도를 설정합니다.")]
    public float moveSpeed = 2f;

   
    private bool playerIsInRange = false;
    private bool conversationHad = false;

    
    private Animator anim;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
       
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (dialogueManager == null)
        {
            Debug.LogError("[CatController] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
        if (catMovePoint == null)
        {
            Debug.LogError("[CatController] 고양이 이동 지점(Cat Move Point)이 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
    }

    void Update()
    {
       
        if (playerIsInRange && Input.GetKeyDown(KeyCode.E) && !dialogueManager.isDialogueActive && !conversationHad)
        {
            StartCatConversation();
        }
    }

    private void StartCatConversation()
    {
       
        conversationHad = true;

        Dialogue dialogue = new Dialogue();

        dialogue.dialogueLines.Add(new DialogueLine { characterName = "고양이", dialogueText = "듣거라." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(고양이가 말을 했다?!)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "고양이?", dialogueText = "인간의 눈은 2개다. 인간에게는 촉수가 달려있지 않다. 저들은 인간이 아니다." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그 순간, 갑자기 격렬한 어지럼증이 느껴졌다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(인간의 눈이 2개라고? 그럴 리 없어. 인간의 눈은 3개인 게 당연한 거잖아? 게다가 촉수가 없다니? 원래 인간은….)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(인간은… 인간은?)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "잠깐, 무슨 소리야. 인간의 눈이 3개라고? 그게 무슨…" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(인간의 눈은 2개야. 그래, 인간의 눈은 2개라고. 그런데 나는 왜 인간의 눈이 3개라는 사실을 이상하게 생각하지 않았지?)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그리고 인간한테 촉수 같은 게 달려 있을 리 없잖아. 왜 여태까지 그걸 눈치채지 못했지?)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(인간의 눈은 2개다. 인간에게는 촉수가 없다. 그렇다면, 나와 방금까지 함께 있던 그들은….)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(머리가 빙글빙글 도는 것 같았다. 나는 서 있지 못하고 바닥에 쓰러졌다.)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "고양이?", dialogueText = "일주일 안에 옥상에 도달해라. 첨탑을 부숴라." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "고양이?", dialogueText = "그리고 두려울 때는 기억해라." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "고양이?", dialogueText = "인간의 눈은 2개라는 것을." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "그 말을 마지막으로, 나는 바닥에서 의식을 잃었다." });

      
        dialogueManager.StartDialogue(dialogue, OnDialogueComplete, true);
    }

   
    private void OnDialogueComplete()
    {
        Debug.Log("고양이와의 대화가 종료되어 이동을 시작합니다.");
        StartCoroutine(MoveCatRoutine());
    }

 
    private IEnumerator MoveCatRoutine()
    {
    
        float direction = Mathf.Sign(catMovePoint.position.x - transform.position.x);

       
        if (anim != null)
        {
          
            anim.SetFloat("moveHorizontal", direction); 
        }
        
        if (spriteRenderer != null && direction != 0)
        {
            
            spriteRenderer.flipX = (direction < 0);
        }

      
        while (Vector3.Distance(transform.position, catMovePoint.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, catMovePoint.position, moveSpeed * Time.deltaTime);
            yield return null; 
        }

   
        transform.position = catMovePoint.position; 
        
        if (anim != null)
        {
           
            anim.SetFloat("moveHorizontal", 0f);
        }
        
        Debug.Log("고양이 이동 완료.");
       
        this.enabled = false; 
    }

    #region 기존 코드 (수정 없음)
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
            if (dialogueManager != null && dialogueManager.isDialogueActive)
            {
                dialogueManager.ForceEndDialogue();
            }
        }
    }
    #endregion
}