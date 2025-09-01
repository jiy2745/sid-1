using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DAY2_manager : MonoBehaviour
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
    private CharacterLook mainCharacterLook;
    private Animator mainCharacterAnimator;
    private CharacterLook reinaLook;


    private Animator reinaAnimator;
    private Animator teacherAnimator;

    void Start()
    {
        InitializeComponents();

        if (mainCharacterLook != null)
        {
            mainCharacterLook.SetLookSprite(false);
        }
        if (reinaLook != null)
        {
            reinaLook.SetLookSprite(false);
        }

        if (playerMovement != null)
        {
            playerMovement.SetMovementActive(false);
            Debug.Log("DAY2 Manager: 컷신 시작. 플레이어 움직임을 비활성화합니다.");
        }
        
        if (mainCharacterAnimator != null)
        {
            mainCharacterAnimator.enabled = false;
            Debug.Log("DAY2 Manager: 컷신 동안 메인 캐릭터의 Animator를 비활성화합니다.");
        }
        
        StartCoroutine(InitialDelay());
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
            case 1: StartReinaApproach(); break;
            case 2: StartDialogueSequence("나", "(레이나의 얼굴에는 3개의 눈이 있었고, 그녀의 등 뒤에는 촉수가 꿈틀대고 있었다)", 3); break;
            case 3: StartDialogueSequence("나", "(그걸 깨닫자 나도 모르게 구역질을 할 뻔했지만, 간신히 참아냈다)", 4); break;
            case 4: StartDialogueSequence("레이나", "…안색이 안 좋네. 무슨 일 있어? (레이나가 정색하며 내게 물었다)", 5); break;
            case 5: StartDialogueSequence("나", "(왜인지 모르겠지만 무언가 들키면 안될 것 같다는 느낌이 들어, 나는 적당히 둘러대었다)", 6); break;
            case 6: StartDialogueSequence("레이나", "흐음, 그렇구나.", 7); break;
            case 7: StartReinaLooksAway(); break;
            case 8: StartTeacherEntrance(); break;
            case 9: StartDialogueSequence("나", "(담임 선생님에게도, 3개의 눈과 촉수가 달려 있었다.)", 10); break;
            case 10: StartDialogueSequence("나", "(나는 그저 덜덜 떨리는 입술을 꽉 깨물하며, 수업 시간을 버티는 것 밖에 할 수 없었다.)", 11); break;
            case 11: StartTeacherMovesAgain(); break;
            case 12: StartDialogueSequence("나", "(통금 시간을 생각해 봤을 때, 자습 시간 동안 주번 업무 중 4개 정도만 할 수 있을 것 같다.)", 13); break;
            case 13: StartDialogueSequence("나", "(또는 업무 대신 레이나와 대화하는 걸 선택할 수도 있을 것이다.)", 14); break;
            case 14: StartDialogueSequence("나", "(주번 업무를 하며, 이 끔찍한 세계와 어제 고양이가 했던 말에 대해 생각해보자.)", 15); break;
          
            case 15: StartFinalTutorialDialoguePart1(); break;
            case 16: StartFinalTutorialDialoguePart2(); break;
            case 17: LoadNextScene(); break;
        }
    }

    #region Sequence Functions (연출 함수)

    void StartInitialDialogue()
    {
        StartDialogueSequence("나", "(정신을 차려보니, 어느새 아침이 되어 있었고 나는 교실에 있었다)", 1);
    }

    void StartReinaApproach()
    {
        isEventSequenceRunning = true;
        
        if (reinaAnimator != null)
        {
            reinaAnimator.SetFloat("lastMoveX", 0);
            reinaAnimator.SetFloat("lastMoveY", 1); 
        }
        
        if (mainCharacterLook != null) mainCharacterLook.SetLookSprite(true);
        if (reinaLook != null) reinaLook.SetLookSprite(true);

        StartDialogueSequence("레이나", "안녕? 오늘은 꽤 일찍 왔네!", 2);
    }

    void StartReinaLooksAway()
    {
        isEventSequenceRunning = true;
        if (mainCharacterLook != null) mainCharacterLook.SetLookSprite(false);
        if (reinaLook != null) reinaLook.SetLookSprite(false);

        if(reinaAnimator != null)
        {
            reinaAnimator.SetFloat("lastMoveX", 0);
            reinaAnimator.SetFloat("lastMoveY", -1); 
        }

        StartDialogueSequence("나", "(레이나는 무언가 의심스럽다는 듯한 표정을 한 채 고개를 돌렸다)", 8);
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
            StartDialogueSequence("나", "(잠시 후, 종소리와 함께 담임 선생님이 들어왔다.)", 9);
        }));
    }
    
    void StartTeacherMovesAgain()
    {
        isEventSequenceRunning = true;
        StartCoroutine(MoveCharacter(teacher, teacherAnimator, Teacher_TargetPos2, teacherMoveSpeed, () => {
            if (teacherAnimator != null)
            {
                teacherAnimator.SetFloat("lastMoveX", 0);
                teacherAnimator.SetFloat("lastMoveY", -1); 
            }
          
            sequenceStep = 12;
            isEventSequenceRunning = false;
        }));
    }

 
    void StartFinalTutorialDialoguePart1()
    {
        StartDialogueSequence("나", "(단, 너무 생각을 깊게 하여 과도한 스트레스를 받지 않도록 하는 게 좋겠지.)", 16);
    }

 
    void StartFinalTutorialDialoguePart2()
    {
        StartDialogueSequence("나", "(물론 그렇다고 생각을 포기해버려서도 안되겠지만.)", 17);
    }
    
    void LoadNextScene()
    {
        isEventSequenceRunning = true;

        if (playerMovement != null)
        {
            playerMovement.SetMovementActive(true);
            Debug.Log("DAY2 Manager: 씬 전환. 플레이어 움직임을 다시 활성화합니다.");
        }
        
        if (mainCharacterAnimator != null)
        {
            mainCharacterAnimator.enabled = true;
            Debug.Log("DAY2 Manager: 씬 전환. 메인 캐릭터의 Animator를 다시 활성화합니다.");
        }
        
        Debug.Log("씬 전환 시작: Day2_2_classroom");
        SceneManager.LoadScene("Day2_2_classroom");
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
            Debug.Log("Main Character가 인스펙터에 연결되지 않아, 'Player' 태그로 탐색합니다.");
            mainCharacter = GameObject.FindGameObjectWithTag("Player");
        }

        if (mainCharacter != null)
        {
            playerMovement = mainCharacter.GetComponent<PlayerMovement>();
            mainCharacterLook = mainCharacter.GetComponent<CharacterLook>();
            mainCharacterAnimator = mainCharacter.GetComponent<Animator>();

            if (playerMovement == null) Debug.LogError("찾은 Main Character 오브젝트에 PlayerMovement 스크립트가 없습니다!", mainCharacter);
            if (mainCharacterLook == null) Debug.LogError("찾은 Main Character 오브젝트에 CharacterLook 스크립트가 없습니다!", mainCharacter);
            if (mainCharacterAnimator == null) Debug.LogError("찾은 Main Character 오브젝트에 Animator 컴포넌트가 없습니다!", mainCharacter);
        }
        else 
        { 
            Debug.LogError("Main Character를 씬에서 찾을 수 없습니다! 'Player' 태그가 지정되어 있는지 확인해주세요."); 
        }

        if (reina != null)
        {
            reinaAnimator = reina.GetComponentInChildren<Animator>();
            reinaLook = reina.GetComponent<CharacterLook>();
            if (reinaLook == null) Debug.LogError("Reina 오브젝트에 CharacterLook 스크립트가 없습니다!", reina);
        }
        else { Debug.LogError("Reina가 DAY2_manager에 연결되지 않았습니다!"); }

        if (teacher != null)
        {
            teacherAnimator = teacher.GetComponentInChildren<Animator>();
        }
        else { Debug.LogError("Teacher가 DAY2_manager에 연결되지 않았습니다!"); }
    }
    
    IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(0.5f);
        StartInitialDialogue();
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