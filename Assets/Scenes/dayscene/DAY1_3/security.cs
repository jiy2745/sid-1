using UnityEngine;

public class security : MonoBehaviour
{
    [Header("대화 관리자 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

    private SpriteRenderer spriteRenderer;
    private bool originalFlipX; 

    private bool playerIsInRange = false;
    private bool conversationHad = false;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalFlipX = spriteRenderer.flipX;
        }
        else
        {
            Debug.LogError("[security] 자식 오브젝트에서 SpriteRenderer 컴포넌트를 찾을 수 없습니다.", this.gameObject);
        }
        
        if (dialogueManager == null)
        {
            Debug.LogError("[security] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
    }

    void Update()
    {
        if (playerIsInRange && Input.GetKeyDown(KeyCode.E) && !dialogueManager.isDialogueActive && !conversationHad)
        {
            StartSecurityDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInRange = true;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = !originalFlipX;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInRange = false;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = originalFlipX;
            }

            if (dialogueManager != null && dialogueManager.isDialogueActive)
            {
                dialogueManager.ForceEndDialogue();
            }
        }
    }
    
    private void StartSecurityDialogue()
    {
        conversationHad = true;

        Dialogue dialogue = new Dialogue();
    
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "수위 할아버지", dialogueText = "이제 들어가는 거냐? 조심히 들어가라." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 고개를 끄덕였다)" });
        
    
        dialogueManager.StartDialogue(dialogue, OnDialogueComplete, true);
    }

    private void OnDialogueComplete()
    {
        Debug.Log("대화가 종료되어 경비가 사라집니다.");
        gameObject.SetActive(false);
    }
}