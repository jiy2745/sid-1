using UnityEngine;

public class reinatalk1 : MonoBehaviour
{
    [Header("대화 관리자 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

    private bool playerIsInRange = false;
 
    private bool conversationHad = false;

    void Start()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("[reinatalk1] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
    }

    void Update()
    {
        
        if (playerIsInRange && Input.GetKeyDown(KeyCode.E) && !dialogueManager.isDialogueActive && !conversationHad)
        {
            StartReinaConversation();
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

            if (dialogueManager != null && dialogueManager.isDialogueActive)
            {
                dialogueManager.ForceEndDialogue();
            }
        }
    }

    private void StartReinaConversation()
    {
    
        conversationHad = true;

        Dialogue dialogue = new Dialogue();


        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "응? 무슨 일이야?" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 주번 업무가 상당히 많아서 통금 시간을 지키기 어렵다고 말했다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "맞아, 나도 저번 주에 그거 때문에 고생 좀 했지." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "사실 주번 업무는 몇 개 빼먹어도 괜찮아. 선생님이 그렇게 꼼꼼히 검사하시지는 않거든." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(레이나에게 그 말을 왜 먼저 안 해줬냐고 물었다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "아하하, 첫날이니까 이왕이면 모든 업무를 다 해보는 게 좋잖아? 그래서 일부러 말 안 한 거야." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "앞으로는 주번 업무를 하다 보면 금세 통금 시간이 다가와 버리는 경우가 많으니까, 어떤 업무를 할지 신중하게 고르는 게 좋을 거야." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "누군가와 대화를 하고 싶다면, 주번 업무를 하면서 중간중간 해야겠지. 대화하는 데 시간이 소모되는 것도 신경 써야 할 테고." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "…그 귀중한 시간을 내어서 나한테 말을 걸어준다면, 조금 기쁠지도." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "아무튼, 꽤 힘들겠지만 그래도 힘내!" });

        dialogueManager.StartDialogue(dialogue, null, true);
    }
}