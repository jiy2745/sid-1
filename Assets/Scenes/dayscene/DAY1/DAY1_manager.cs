using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class DAY1_manager : MonoBehaviour
{
    [Header("캐릭터 연결")]
    public CharacterLook mainCharacterLook;
    public CharacterLook reinaStillLook;
    public GameObject teacher;

    [Header("선생님 이동 설정")]
    public Transform teacherTargetPosition;
    public Transform teacherExitPosition; 
    public float teacherMoveSpeed = 3f;

    [Header("대화 관리자 연결")]
    public day1_dialogmanager dialogueManager;

   
    [Header("음악 설정")]
    public AudioClip backgroundMusic;

    private int sequenceStep = 0;
    
    private bool isEventSequenceRunning = false;

    void Start()
    {
       
        if (MusicManager.instance != null)
        {
           
            if (backgroundMusic != null)
            {
                
                MusicManager.instance.PlayMusicDirectly(backgroundMusic);
            }
            else
            {
                Debug.LogWarning("DAY1_manager의 backgroundMusic 변수에 오디오 클립이 지정되지 않았습니다!", this.gameObject);
            }
        }
        else
        {
            Debug.LogError("씬에 MusicManager가 없습니다! 음악을 재생할 수 없습니다.", this.gameObject);
        }

        
        if (dialogueManager == null)
            Debug.LogError("DAY1_manager에 day1_dialogmanager가 연결되지 않았습니다!", this.gameObject);
        if (teacher == null || teacherTargetPosition == null || teacherExitPosition == null)
            Debug.LogError("선생님 또는 선생님 목표 위치 중 하나가 DAY1_manager에 연결되지 않았습니다!", this.gameObject);
    }
    
    void Update()
    {
        if (!isEventSequenceRunning && !dialogueManager.isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            switch (sequenceStep)
            {
                case 0: StartReinaDialogue(); break;
                case 1: StartTeacherSequence(); break;
                case 2: StartTeacherDialoguePart2(); break;
                case 3: StartTeacherDialoguePart3(); break;
                case 4: StartTeacherExitSequence(); break;
                case 5: StartPlayerRequestDialogue(); break;
                case 6: StartReinaExplanation1(); break;
                case 7: StartReinaExplanation2(); break;
                case 8: StartReinaExplanation3(); break;
                case 9: StartReinaExplanation4(); break;
                case 10: StartReinaExplanation5(); break;
                case 11: StartPlayerThanksDialogue(); break;
                case 12: LoadNextScene(); break; 
            }
        }
    }
    
    #region Dialogue Sequences
    void StartReinaDialogue()
    {
        isEventSequenceRunning = true; 
        mainCharacterLook.SetLookSprite(true);
        reinaStillLook.SetLookSprite(true);
        
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "안녕! 오늘도 늦지 않고 왔구나? 아슬아슬했네!" });
        dialogueManager.StartDialogue(dialogue, () => { sequenceStep = 1; isEventSequenceRunning = false; });
    }

    void StartTeacherSequence()
    {
        isEventSequenceRunning = true; 
        StartCoroutine(MoveTeacher(teacherTargetPosition, null));

        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "담임 선생님", dialogueText = "여러분, 좋은 아침입니다. 오늘은 일주일의 시작인 월요일이니, 힘차게 조례를 시작해보도록 하죠." });
        dialogueManager.StartDialogue(dialogue, () => { sequenceStep = 2; isEventSequenceRunning = false; });
    }
    
    void StartTeacherDialoguePart2()
    {
        isEventSequenceRunning = true;
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "담임 선생님", dialogueText = "우선 알려드릴 사항이 있습니다. 일주일이 지났으니 이제 주번이 다음 순서로 넘어갈 예정입니다." });
        dialogueManager.StartDialogue(dialogue, () => { sequenceStep = 3; isEventSequenceRunning = false; });
    }
    
    void StartTeacherDialoguePart3()
    {
        isEventSequenceRunning = true;
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "담임 선생님", dialogueText = "저번 주에 주번을 맡았던 레이나 학생은 수업이 끝난 후 자습시간에 인수인계를 잘해주시길 바랍니다." });
        dialogueManager.StartDialogue(dialogue, () => { sequenceStep = 4; isEventSequenceRunning = false; });
    }

    void StartTeacherExitSequence()
    {
        isEventSequenceRunning = true;
        Debug.Log("선생님 퇴장 시퀀스 시작");
        StartCoroutine(MoveTeacher(teacherExitPosition, () => {
             Debug.Log("선생님 퇴장 완료");
             sequenceStep = 5;
             isEventSequenceRunning = false;
        }));
    }
    
    void StartPlayerRequestDialogue()
    {
        isEventSequenceRunning = true;
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(레이나에게 주번 일에 대해 알려달라고 부탁했다)" });
        dialogueManager.StartDialogue(dialogue, () => { sequenceStep = 6; isEventSequenceRunning = false; });
    }

    void StartReinaExplanation1()
    {
        isEventSequenceRunning = true;
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "맞다, 아침에 이번 주 주번은 너라고 했지? 우리 반은 한 명씩 주번을 돌아가면서 맡으니까." });
        dialogueManager.StartDialogue(dialogue, () => { sequenceStep = 7; isEventSequenceRunning = false; });
    }

    void StartReinaExplanation2()
    {
        isEventSequenceRunning = true;
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "좋아, 그러면 주번이 어떤 걸 해야 하는지 알려줄게!" });
        dialogueManager.StartDialogue(dialogue, () => { sequenceStep = 8; isEventSequenceRunning = false; });
    }

    void StartReinaExplanation3()
    {
        isEventSequenceRunning = true;
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "일단 주번은 학교 뒤편의 우리 반 담당 구역을 관리해야 해. 그곳에 있는 토끼들에게 밥을 주고, 식물에 물을 주어야 하지." });
        dialogueManager.StartDialogue(dialogue, () => { sequenceStep = 9; isEventSequenceRunning = false; });
    }

    void StartReinaExplanation4()
    {
        isEventSequenceRunning = true;
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "또, 오늘 하루 일어난 일에 대해 학급 일지를 쓰고, 우리 반 학생들이 제출한 과제 정리하는 것도 해야 해." });
        dialogueManager.StartDialogue(dialogue, () => { sequenceStep = 10; isEventSequenceRunning = false; });
    }

    void StartReinaExplanation5()
    {
        isEventSequenceRunning = true;
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "그리고 교실 바닥을 청소하고, 학급 문고를 정리하는 것도 잊어선 안 되지." });
        dialogueManager.StartDialogue(dialogue, () => { sequenceStep = 11; isEventSequenceRunning = false; });
    }

    void StartPlayerThanksDialogue()
    {
        isEventSequenceRunning = true;
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(레이나에게 설명해줘서 고맙다고 말했다)" });
        dialogueManager.StartDialogue(dialogue, () => { 
            sequenceStep = 12;
            isEventSequenceRunning = false; 
        });
    }
    
    void LoadNextScene()
    {
        isEventSequenceRunning = true; 
        Debug.Log("씬 로딩: Day1_2_classroom");
        SceneManager.LoadScene("Day1_2_classroom");
    }
    
    IEnumerator MoveTeacher(Transform targetPos, System.Action onCompleted)
    {
        while (Vector3.Distance(teacher.transform.position, targetPos.position) > 0.1f)
        {
            teacher.transform.position = Vector3.MoveTowards(
                teacher.transform.position,
                targetPos.position,
                teacherMoveSpeed * Time.deltaTime
            );
            yield return null;
        }
        teacher.transform.position = targetPos.position;
        onCompleted?.Invoke();
    }
    #endregion
}