using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class DAY3_manager : MonoBehaviour
{
    [Header("캐릭터 연결")]
    public GameObject mainCharacter;
    public GameObject reina;
    public GameObject teacher;

    [Header("이동 지점 연결")]
    public Transform Teacher_TargetPos;
    public Transform Teacher_TargetPos2;

    [Header("이동 속도 설정")]
    public float teacherMoveSpeed = 3f;

    [Header("관리자 연결")]
    public day1_dialogmanager dialogueManager;

    private int sequenceStep = 0;
    private bool isEventSequenceRunning = false;
    
   
    private PlayerMovement playerMovement;
    private Animator mainCharacterAnimator;
    private Animator reinaAnimator;
    private Animator teacherAnimator;
   
    private CharacterLook mainCharacterLook;
    private CharacterLook reinaLook;

    void Start()
    {
        InitializeComponents();

        if (playerMovement != null)
        {
            playerMovement.SetMovementActive(false);
            Debug.Log("DAY3 Manager: 컷신 시작. 플레이어 움직임을 비활성화합니다.");
        }
        
        if (mainCharacterAnimator != null)
        {
            mainCharacterAnimator.enabled = false;
            Debug.Log("DAY3 Manager: 컷신 동안 메인 캐릭터의 Animator를 비활성화합니다.");
        }
        
        StartCoroutine(BeginDay3Sequence());
    }

    void Update()
    {
        if (!isEventSequenceRunning && !dialogueManager.isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            ProceedSequence();
        }
    }

    private void ProceedSequence()
    {
        switch (sequenceStep)
        {
          
            case 1: StartDialogueSequence("나", "(…그러자 순간적으로 모든 걸 포기하고 체념하고 싶다는 생각이 들었다)", 2); break;
            case 2: StartDialogueSequence("나", "(긴장을 놓치 않도록 조심하자)", 3); break;

        
            case 10: StartDialogueSequence("나", "(…오늘은 이 세계의 진실을 파헤치는 것보다는, 조금 쉬는 게 좋겠다)", 3); break;

         
            case 3: StartReinaConversation(); break; 
            case 4: StartDialogueSequence("레이나", "너무 힘들면 수업 시간에 좀 자도 돼. 내가 선생님께 안 혼나도록 해줄 테니까.", 5); break;
            case 5: StartDialogueSequence("나", "(나는 그녀에게 적당히 감사 인사를 전했다)", 6); break;
            case 6: EndReinaConversationAndStartNext(); break;
            case 7: StartTeacherEntrance(); break;
            case 8: StartDialogueSequence("나", "(그 후 평소처럼 조례와 수업 시간이 진행되었다)", 9); break;
            case 9: StartTeacherMovesToDesk(); break;
            case 11: StartDialogueSequence("나", "(우선 어젯밤에 약속한 대로 에리카와 대화해보자)", 12); break;
            case 12: LoadNextScene(); break;
        }
    }

    #region Sequence Functions (연출 함수)

    private IEnumerator BeginDay3Sequence()
    {
        yield return new WaitUntil(() => GameManager.instance != null);

        int enlightenment = GameManager.instance.enlightenmentMeter;

        if (enlightenment < 30)
        {
            StartDialogueSequence("나", "(부드러운 바람이 스쳐 지나가는 것이 느껴져서, 기분 좋은 아침이었다)", 1);
        }
        else if (enlightenment >= 80)
        {
            StartDialogueSequence("나", "(교실에 들어서자 숨이 막혀온다. 불안감이 나를 짓누른다)", 10);
        }
        else
        {
            StartReinaConversation(); 
        }
    }

    void StartReinaConversation()
    {
   
        if (mainCharacterLook != null) mainCharacterLook.SetLookSprite(true);
        if (reinaLook != null) reinaLook.SetLookSprite(true);

        StartDialogueSequence("레이나", "안녕? 오늘은 어제보다도 더 지친 얼굴이네.", 4);
    }
    
    void EndReinaConversationAndStartNext()
    {
       
        if (mainCharacterLook != null) mainCharacterLook.SetLookSprite(false);
        if (reinaLook != null) reinaLook.SetLookSprite(false);
        
        StartDialogueSequence("나", "(종소리와 함께 담임 선생님이 들어왔다)", 7);
    }
    
    void StartTeacherEntrance()
    {
        isEventSequenceRunning = true;
        StartCoroutine(MoveCharacter(teacher, teacherAnimator, Teacher_TargetPos, teacherMoveSpeed, () => {
            if (teacherAnimator != null)
            {
                teacherAnimator.SetFloat("lastMoveX", 0);
                teacherAnimator.SetFloat("lastMoveY", -1); 
            }
            StartDialogueSequence("담임선생님", "여러분, 오늘도 좋은 아침입니다.", 8);
        }));
    }
    
    void StartTeacherMovesToDesk()
    {
        isEventSequenceRunning = true;
        StartCoroutine(MoveCharacter(teacher, teacherAnimator, Teacher_TargetPos2, teacherMoveSpeed, () => {
            if (teacherAnimator != null)
            {
                teacherAnimator.SetFloat("lastMoveX", 0);
                teacherAnimator.SetFloat("lastMoveY", -1);
            }
            StartDialogueSequence("나", "(수업 시간이 끝나고, 이제 내가 주번 업무를 해야 할 자습 시간이 찾아왔다)", 11);
        }));
    }
    
    void LoadNextScene()
    {
        isEventSequenceRunning = true;

        if (playerMovement != null)
        {
            playerMovement.SetMovementActive(true);
            Debug.Log("DAY3 Manager: 씬 전환. 플레이어 움직임을 다시 활성화합니다.");
        }
        
        if (mainCharacterAnimator != null)
        {
            mainCharacterAnimator.enabled = true;
            Debug.Log("DAY3 Manager: 씬 전환. 메인 캐릭터의 Animator를 다시 활성화합니다.");
        }
        
        Debug.Log("씬 전환 시작: Day3_2_classroom");
        SceneManager.LoadScene("Day3_2_classroom");
    }

    #endregion

    #region Helper Functions (보조 함수)

    void StartDialogueSequence(string characterName, string text, int nextStep)
    {
        isEventSequenceRunning = true;
        Dialogue dialogue = new Dialogue();
        dialogue.dialogueLines.Add(new DialogueLine { characterName = characterName, dialogueText = text });
        dialogueManager.StartDialogue(dialogue, () => {
            sequenceStep = nextStep;
            isEventSequenceRunning = false;
        }, true);
    }

    void InitializeComponents()
    {
       
        if (mainCharacter == null)
        {
            mainCharacter = GameObject.FindGameObjectWithTag("Player");
        }

        if (mainCharacter != null)
        {
            playerMovement = mainCharacter.GetComponent<PlayerMovement>();
            mainCharacterAnimator = mainCharacter.GetComponent<Animator>();
            mainCharacterLook = mainCharacter.GetComponent<CharacterLook>(); // CharacterLook 컴포넌트 찾기

            if (playerMovement == null) Debug.LogError("Main Character에 PlayerMovement 스크립트가 없습니다!", mainCharacter);
            if (mainCharacterAnimator == null) Debug.LogError("Main Character에 Animator 컴포넌트가 없습니다!", mainCharacter);
            if (mainCharacterLook == null) Debug.LogError("Main Character에 CharacterLook 스크립트가 없습니다!", mainCharacter);
        }
        else 
        { 
            Debug.LogError("씬에서 'Player' 태그를 가진 Main Character를 찾을 수 없습니다!"); 
        }

       
        if (reina != null)
        {
            reinaAnimator = reina.GetComponentInChildren<Animator>();
            reinaLook = reina.GetComponent<CharacterLook>(); // CharacterLook 컴포넌트 찾기
            if (reinaLook == null) Debug.LogError("Reina 오브젝트에 CharacterLook 스크립트가 없습니다!", reina);
        }
        else { Debug.LogError("Reina가 DAY3_manager에 연결되지 않았습니다!"); }

      
        if (teacher != null)
        {
            teacherAnimator = teacher.GetComponentInChildren<Animator>();
        }
        else { Debug.LogError("Teacher가 DAY3_manager에 연결되지 않았습니다!"); }
    }
    
    IEnumerator MoveCharacter(GameObject character, Animator anim, Transform targetPos, float speed, Action onCompleted)
    {
        if (anim != null)
        {
            Vector2 direction = (targetPos.position - character.transform.position).normalized;
            anim.SetFloat("moveX", direction.x);
            anim.SetFloat("moveY", direction.y);
            anim.SetFloat("speed", 1f);
        }

        while (Vector3.Distance(character.transform.position, targetPos.position) > 0.1f)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, targetPos.position, speed * Time.deltaTime);
            yield return null;
        }

        character.transform.position = targetPos.position;

        if (anim != null)
        {
            anim.SetFloat("speed", 0f);
        }
        
        onCompleted?.Invoke();
    }
    
    #endregion
}