using UnityEngine;

public class EricaTalk2 : MonoBehaviour
{
    [Header("대화 관리자 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

    private Animator anim;
    private bool playerIsInRange = false;
  
    private GameObject playerObject; 

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        
        if (dialogueManager == null)
        {
            Debug.LogError("[EricaTalk2] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
    }

    void Update()
    {
     
        if (playerIsInRange && Input.GetKeyDown(KeyCode.E) && !dialogueManager.isDialogueActive)
        {
            StartEricaDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지! 에리카가 쳐다봅니다.");
            playerIsInRange = true;
         
            playerObject = other.gameObject; 
            if (anim != null)
            {
                anim.SetBool("PlayerIsNear", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 벗어남, 에리카가 원래 상태로 돌아갑니다.");
            playerIsInRange = false;
           
            playerObject = null; 
            if (anim != null)
            {
                anim.SetBool("PlayerIsNear", false);
            }

            if (dialogueManager != null && dialogueManager.isDialogueActive)
            {
                dialogueManager.ForceEndDialogue();
            }
        }
    }
    
    private void StartEricaDialogue()
    {
    
        if (playerObject == null) return;

        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
        
      
        if (playerMovement != null)
        {
            playerMovement.SetMovementActive(false);
            Debug.Log("대화 시작. 플레이어 움직임을 비활성화합니다.");
        }

        Dialogue dialogue = new Dialogue();
        
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "약속한대로 자습 시간에 말을 걸어줬네. 고마워." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "다른 시간대에 말을 걸었으면 상당히 곤란했을 거거든. 그러면 다른 학생들이 우리들의 대화에 관심을 갖게 될 테니까." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "앞으로도 될 수 있으면 자습 시간에 같이 이야기하도록 하자." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 그녀에게 어제 만났던 그 괴물에 대해 물었다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "그 괴물은 평소에는 '수위 아저씨'로 인식되는 존재이지만, 이 세계의 진실을 깨달은 존재에게는 괴물로 보이는 녀석이야." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "그리고 그 녀석은 자신을 괴물로 인식하는 존재를 죽이려 들지." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "통금 시간이 다가오면 그 괴물이 복도를 돌아다니니, 조심하는 편이 좋을 거야." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그녀에게 괴물을 쫓아낼 방법을 물어봤다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "여기 있는 괴물들은 다들 불을 두려워하는 특징이 있어." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "자, 여기. 혹시 모르니까 너한테도 성냥개비를 하나 나눠줄게." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "", dialogueText = "(에리카에게서 성냥개비를 받았다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 그녀에게 수위 아저씨 괴물 말고도 다른 괴물이 있냐고 물었다.)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "응. 이 학교에는 자신의 본모습을 감추고 있는 괴물들이 많이 있어." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "너도 조심하는 게 좋을 거야." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(에리카의 말을 듣자, 온 몸에 소름이 끼쳤다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "너도 알다시피, 우리는 이번 주 안에 옥상으로 가서 첨탑을 부숴야 해." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 첨탑을 부수지 못하면 어떻게 되냐고 물었다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "첨탑을 부수지 못하면 어떻게 되냐고? 간단해. 이 뒤틀린 세계에 영원히 갇히게 되지." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "우린 현재 원래 있던 세계에서 끌려나와 이 뒤틀린 세계에 갇힌 거야. 그리고 이 세계의 닻 역할을 하는 게 옥상의 첨탑이고." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "첨탑을 부수면 우리는 원래 있던 세계로 돌아갈 수 있어. 하지만 시간 내에 부수지 못하면 이 세계가 우리를 완전히 침식시킬 거야." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "그러니 최대한 빨리 첨탑을 부숴야 해. 이해됐지?" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 고개를 끄덕였다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "에리카", dialogueText = "앞으로 자습 시간이 되면 나한테 와. 같이 첨탑을 부술 방법을 생각해 보자." });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 그녀와의 대화를 마치고, 주번 업무를 시작하기로 했다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(에리카와의 대화에 시간을 꽤 많이 써서, 오늘은 주번 업무를 3개까지밖에 못 할 것 같다)" });
        
      
        System.Action onDialogueComplete = () => {
            if (playerMovement != null)
            {
                playerMovement.SetMovementActive(true);
                Debug.Log("대화 종료. 플레이어 움직임을 다시 활성화합니다.");
            }
        };

        dialogueManager.StartDialogue(dialogue, onDialogueComplete); 
    }
}