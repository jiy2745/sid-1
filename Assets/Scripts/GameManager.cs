using UnityEngine;
using UnityEngine.Events; 


public class GameManager : MonoBehaviour, IDataPersistence
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

    // (08/04 진성민)
    public void SweepFloor()
    {
        if (CanAct())
        {
            Debug.Log("빗자루로 바닥을 청소했다. 계몽 수치가 3 감소합니다.");

            // 빗자루질은 보통 평범한 활동이므로 계몽 수치를 약간 감소시키기
            enlightenmentMeter -= 3;
            UseAction(); // 행동 횟수 차감
        }
    }

    // 
    public void CollectHomework()
    {
        if (CanAct())
        {
            Debug.Log("친구들의 과제를 걷었다.");

            // 30% 확률로 이상한 과제를 발견하는 이벤트 (확률논의 필요)
            if (Random.Range(0, 100) < 30)
            {
                Debug.Log("한 친구의 과제가 알 수 없는 언어로 적혀있는 것을 발견했다...");
                Debug.Log("계몽 수치가 5 증가합니다.");
                enlightenmentMeter += 5; // 계몽 수치 증가
            }
            else
            {
                // 특별한 이벤트가 없을 경우
                Debug.Log("오늘은 별다른 이상한 점은 없었다.");
            }

            UseAction(); // 행동 횟수 차감
        }
    }
    
    //(08/04 진성민)
    public void OrganizeBookshelf()
    {
        if (CanAct())
        {
            Debug.Log("학급 문고를 정리했다.");

            // 현재 날짜에 따라 다른 이벤트 발생
            switch (currentDay)
            {
                case 1:
                    // 1일차: 특별한 이벤트 없음, 계몽 수치만 약간 증가
                    Debug.Log("책을 정리하자 머리가 약간 아파왔다. 계몽 수치가 5 증가합니다.");
                    enlightenmentMeter += 5;
                    break;

                case 2:
                    // 2일차: 토끼 마법 스크롤 발견
                    Debug.Log("책장 구석에서 이상한 양피지를 발견했다! (토끼 마법 스크롤 획득)");
                    // 여기에 '토끼 마법 스크롤' 아이템을 획득하는 로직을 추가해야 함(추후에)
                    // InventoryManager.instance.AddItem("rabbit_scroll"); 이런식으로
                    enlightenmentMeter += 5;
                    break;

                default:
                    // 3일차 이후: 일정 확률로 이상한 책 발견
                    if (Random.Range(0, 100) < 30) // 30% 확률(임의값. 조정 논의 필요)
                    {
                        Debug.Log("알 수 없는 언어로 쓰인 책을 발견했다. 머리가 깨질 것 같다!");
                        enlightenmentMeter += 10; // 더 큰 폭으로 증가
                    }
                    else
                    {
                        Debug.Log("오늘은 별다른 것을 찾지 못했다.");
                    }
                    break;
            }

            UseAction(); // 행동 횟수 차감
        }
    }
    
    public void WaterSprout()
    {
        if (CanAct())
        {
            Debug.Log("새싹에 물을 주었다. 계몽 수치가 3 감소합니다.");
            enlightenmentMeter -= 3;
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

    public void LoadData(GameData data)
    {
        this.currentDay = data.currentDay;
        this.actionsLeft = data.actionsLeft;
        this.enlightenmentMeter = data.enlightenmentMeter;
        this.girlFavorability = data.girlFavorability;
        this.glassesFavorability = data.glassesFavorability;
        this.rabbitFavorability = data.rabbitFavorability;
    }

    public void SaveData(GameData data)
    {
        data.currentDay = this.currentDay;
        data.actionsLeft = this.actionsLeft;
        data.enlightenmentMeter = this.enlightenmentMeter;
        data.girlFavorability = this.girlFavorability;
        data.glassesFavorability = this.glassesFavorability;
        data.rabbitFavorability = this.rabbitFavorability;

        Debug.Log("Saved game data");
    }
}