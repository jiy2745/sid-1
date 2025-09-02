using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameTrigger : MonoBehaviour
{
    [Header("대화 관리자 연결")]
    [Tooltip("씬에 있는 Dialogue Manager를 연결해주세요.")]
    public day1_dialogmanager dialogueManager;

 
    private bool eventTriggered = false;

    void Start()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("[MiniGameTrigger] Dialogue Manager가 연결되지 않았습니다. Inspector에서 설정해주세요.", this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
 
        if (other.CompareTag("Player") && !eventTriggered && !dialogueManager.isDialogueActive)
        {
          
            eventTriggered = true;
            StartMiniGameIntro();
        }
    }

    private void StartMiniGameIntro()
    {
        Debug.Log("미니게임 인트로 시퀀스를 시작합니다.");
        Dialogue dialogue = new Dialogue();

        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(복도 너머로 무언가 꿈틀거리는 것이 보였다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(분명 어제까지만 해도 수위 아저씨로 보였던 '그것'은, 이제 흉측한 모습을 한 채 나를 바라보고 있었다)" });
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(괴물은 나를 잠시 응시하더니, 내게 다가오기 시작했다)" });

     
        dialogueManager.StartDialogue(dialogue, LoadMiniGameScene, true);
    }

  
    private void LoadMiniGameScene()
    {
        Debug.Log("미니게임 인트로 종료. 기존 플레이어를 파괴하고 NightScene2_3 씬으로 전환합니다.");

        
        GameObject player = GameObject.FindWithTag("Player");

       
        if (player != null)
        {
            
            Destroy(player);
        }
        else
        {
            Debug.LogWarning("파괴할 플레이어를 찾지 못했습니다.");
        }

       
        SceneManager.LoadScene("NightScene2_3");
    }
}