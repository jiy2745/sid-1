using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic; // HashSet을 사용하기 위해 추가

public class GameManager : MonoBehaviour, IDataPersistence
{
    // 싱글턴 인스턴스: 어떤 스크립트에서도 이 인스턴스를 통해 GameManager에 접근 가능
    public static GameManager instance;

    [Header("게임 상태 변수")]
    public int currentDay = 1;
    public int actionsLeft = 4;
    public int enlightenmentMeter = 50;

    [Header("NPC 호감도")]
    public int girlFavorability = 0;
    public int glassesFavorability = 0;
    public int rabbitFavorability = 0;

    // 상태가 변경될 때 UI업뎃 등을 위해 사용할 수 있는 이벤트
    public UnityEvent onStateChanged;

    // (08/18 기존 interactable 스크립트 버그 수정을 위해 gamemanager에서 정보 가져오는 방식으로)
    [Header("상호작용 기록")]
    private HashSet<string> interactedObjectIds = new HashSet<string>();

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
            enlightenmentMeter -= 3;
            UseAction();
        }
    }

    public void CollectHomework()
    {
        if (CanAct())
        {
            Debug.Log("친구들의 과제를 걷었다.");
            if (Random.Range(0, 100) < 30)
            {
                Debug.Log("한 친구의 과제가 알 수 없는 언어로 적혀있는 것을 발견했다...");
                Debug.Log("계몽 수치가 5 증가합니다.");
                enlightenmentMeter += 5;
            }
            else
            {
                Debug.Log("오늘은 별다른 이상한 점은 없었다.");
            }
            UseAction();
        }
    }
    
    //(08/04 진성민)
    public void OrganizeBookshelf()
    {
        if (CanAct())
        {
            Debug.Log("학급 문고를 정리했다.");
            switch (currentDay)
            {
                case 1:
                    Debug.Log("책을 정리하자 머리가 약간 아파왔다. 계몽 수치가 5 증가합니다.");
                    enlightenmentMeter += 5;
                    break;
                case 2:
                    Debug.Log("책장 구석에서 이상한 양피지를 발견했다! (토끼 마법 스크롤 획득)");
                    enlightenmentMeter += 5;
                    break;
                default:
                    if (Random.Range(0, 100) < 30)
                    {
                        Debug.Log("알 수 없는 언어로 쓰인 책을 발견했다. 머리가 깨질 것 같다!");
                        enlightenmentMeter += 10;
                    }
                    else
                    {
                        Debug.Log("오늘은 별다른 것을 찾지 못했다.");
                    }
                    break;
            }
            UseAction();
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

    // --- Interactable 상태 관리 함수 ---
    public bool IsInteracted(string objectId)
    {
        if (string.IsNullOrEmpty(objectId)) return false;
        return interactedObjectIds.Contains(objectId);
    }

    public void SetInteracted(string objectId)
    {
        if (string.IsNullOrEmpty(objectId)) return;
        if (!interactedObjectIds.Contains(objectId))
        {
            interactedObjectIds.Add(objectId);
        }
    }

    // -----------------내부 관리 함수들----------------
    private void UseAction()
    {
        actionsLeft--;
        Debug.Log("남은 행동 횟수: " + actionsLeft);
        onStateChanged.Invoke();
        if (actionsLeft <= 0)
        {
            Debug.Log("오늘의 행동 횟수를 모두 사용했습니다. 밤으로 넘어갑니다.");
        }
    }

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

    public void StartNewDay()
    {
        currentDay++;
        actionsLeft = 4;
        Debug.Log(currentDay + "일차 시작. 남은 행동 횟수: " + actionsLeft);
        onStateChanged.Invoke();
    }

    // --- 데이터 저장/불러오기 ---
    public void LoadData(GameData data)
    {
        this.currentDay = data.currentDay;
        this.actionsLeft = data.actionsLeft;
        this.enlightenmentMeter = data.enlightenmentMeter;
        this.girlFavorability = data.girlFavorability;
        this.glassesFavorability = data.glassesFavorability;
        this.rabbitFavorability = data.rabbitFavorability;
        // 상호작용 기록 불러오기
        this.interactedObjectIds = new HashSet<string>(data.interactedObjectIds);
        Debug.LogWarning($"[GameManager] LoadData가 호출되었습니다. actionsLeft가 <color=yellow>{this.actionsLeft}</color>로 설정됩니다.");
    }

    // This method is typcially called by the DataPersistenceManager at scene start or when saving the game
    public void SaveData(GameData data)
    {
        data.lastSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.currentDay = this.currentDay;
        data.actionsLeft = this.actionsLeft;
        data.enlightenmentMeter = this.enlightenmentMeter;
        data.girlFavorability = this.girlFavorability;
        data.glassesFavorability = this.glassesFavorability;
        data.rabbitFavorability = this.rabbitFavorability;
        // 상호작용 기록 저장하기
        data.interactedObjectIds = new List<string>(this.interactedObjectIds);
        Debug.Log("Saved game data");
    }
}
