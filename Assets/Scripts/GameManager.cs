using UnityEngine;
using UnityEngine.Events; 


public class GameManager : MonoBehaviour
{

    // 싱글턴 인스턴스: 어떤 스크립트에서도 이 인스턴스를 통해 GameManager에 접근 가능
    public static GameManager instance;

    [Header("게임 상태 변수")]
    public int currentDay = 1;          // 현재 날짜
    public int actionsLeft = 4;         // 하루에 남은 행동 횟수
    public int enlightenmentMeter = 50; // 계몽 수치

    [Header("NPC 호감도")]
    public int girlFavorability = 0;    // 옆자리 소녀 호감도
    public int glassesFavorability = 0; // 안경 소녀 호감도
    public int rabbitFavorability = 0;  // 토끼 호감도


    // 상태가 변경될 때 UI업뎃 등을 위해 사용할 수 있는 이벤트
    public UnityEvent onStateChanged;



    // 게임이 시작될 때 가장 먼저 호출
    void Awake()
    {
        // 싱글턴 패턴 설정: 씬에 GameManager가 하나만 존재하도록 보장
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 이 오브젝트는 파괴되지 않도록 함
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 새로 생긴건 파괴
        }
    }



    // --------- 상호작용 함수들 --------
    // 이 함수들을 Interactable 오브젝트에서 연결하여 작동하는 구조

    public void WriteJournal()
    {
        if (CanAct())
        {
            Debug.Log("학급 일지를 작성했다. 계몽 수치가 5 증가합니다.");
            enlightenmentMeter += 5;
            UseAction();
        }
    }

    public void TalkToDeskmate()
    {
        if (CanAct())
        {
            Debug.Log("옆자리 소녀와 대화했다. 호감도가 1 증가하고 계몽 수치가 3 감소합니다.");
            girlFavorability++;
            enlightenmentMeter -= 3;
            UseAction();
        }
    }

    public void FeedRabbit()
    {
        if (CanAct())
        {
            Debug.Log("토끼에게 밥을 주었다. 호감도가 1 증가합니다.");
            rabbitFavorability++;
            UseAction();
        }
    }



    // -----------------내부 관리 함수들----------------

    // 행동하고, 상태 변경 이벤트를 호출하는 함수
    private void UseAction()
    {
        actionsLeft--;
        Debug.Log("남은 행동 횟수: " + actionsLeft);
        onStateChanged.Invoke(); // UI 등을 위해 이벤트를 발생시킴

        if (actionsLeft <= 0)
        {
            Debug.Log("오늘의 행동 횟수를 모두 사용했습니다. 밤으로 넘어갑니다.");
            
            // 여기에 밤으로 넘어가는 로직을 추가하면 될듯함
            // ChangeToNight()....;

        }
    }

    // 행동할 수 있는지 확인하는 함수
    private bool CanAct()
    {
        if (actionsLeft > 0)
        {
            return true;
        }
        else
        {
            Debug.Log("행동 횟수가 남아있지 않습니다.");
            return false;
        }
    }



    // 새 날이 시작될 때 호출할 함수(예시 프로토타입. 이것도 더 논의 필요)
    public void StartNewDay()
    {
        currentDay++;
        actionsLeft = 4; // 행동 횟수 초기화
        Debug.Log(currentDay + "일차 시작. 남은 행동 횟수: " + actionsLeft);
        onStateChanged.Invoke();
    }
}