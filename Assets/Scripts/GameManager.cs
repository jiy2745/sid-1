using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    public string nextPlayerSpawnPointName;

    [Header("게임 상태 변수")]
    public int currentDay = 1;
    public int actionsLeft = 4;
    public int enlightenmentMeter = 50;

    [Header("NPC 호감도")]
    public int girlFavorability = 0;
    public int glassesFavorability = 0;
    public int rabbitFavorability = 0;

    public UnityEvent onStateChanged;

    [Header("상호작용 기록")]
    private HashSet<string> interactedObjectIds = new HashSet<string>();

    private day1_dialogmanager dialogueManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ShowActionDialogue(string text, string characterName = "나")
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<day1_dialogmanager>();
        }

        if (dialogueManager == null)
        {
            Debug.LogError("씬에 day1_dialogmanager가 없습니다! 대사를 표시할 수 없습니다.");
            return;
        }

        if (dialogueManager.isDialogueActive) return;

        Dialogue dialogue = new Dialogue();
        DialogueLine line = new DialogueLine
        {
            characterName = characterName,
            dialogueText = text
        };
        dialogue.dialogueLines.Add(line);
        
        dialogueManager.StartDialogue(dialogue, null, true);
    }

    public void WriteJournal()
    {
        if (CanAct())
        {
            ShowActionDialogue("(오늘 있던 일을 되돌아보며 학급 일지를 작성했다.)");

            // 1일차에는 계몽 수치 변경 없음
            if (currentDay > 1)
            {
                enlightenmentMeter += 5;
            }
            
            UseAction();
        }
    }

    public void TalkToDeskmate()
    {
        if (CanAct())
        {
            ShowActionDialogue("주번 활동을 하고 있구나? 멋있어!" , "레이나");
            
            switch (currentDay)
            {
                case 1:
                    // 1일차: 레이나 호감도 +1, 계몽 수치 변경 없음
                    girlFavorability++;
                    break;
                case 2:
                    // 2일차: 레이나 호감도 +1, 계몽 수치 -8
                    girlFavorability++;
                    enlightenmentMeter -= 8;
                    break;
                default:
                    // 그 외의 날: 기존 로직
                    girlFavorability++;
                    enlightenmentMeter -= 3;
                    break;
            }
            UseAction();
        }
    }

    public void FeedRabbit()
    {
        if (CanAct())
        {
            ShowActionDialogue("(토끼에게 밥을 주었다.)");
            rabbitFavorability++;
            
            // 2일차에만 계몽 수치 변경
            if (currentDay == 2)
            {
                enlightenmentMeter += 5;
            }
            
            UseAction();
        }
    }

    public void SweepFloor()
    {
        if (CanAct())
        {
            ShowActionDialogue("(교실 바닥을 열심히 쓸었다.)");
            
            // 1일차에는 계몽 수치 변경 없음
            if (currentDay > 1)
            {
                if (currentDay == 2)
                {
                    enlightenmentMeter -= 5; // 2일차
                }
                else
                {
                    enlightenmentMeter -= 3; // 그 외의 날
                }
            }

            UseAction();
        }
    }

    public void CollectHomework()
    {
        if (CanAct())
        {
            ShowActionDialogue("(학생들이 제출한 과제를 순서대로 정리했다.)");

            // 1일차에는 계몽 수치 변경 없음
            if (currentDay > 1)
            {
                if (Random.Range(0, 100) < 30)
                {
                    enlightenmentMeter += 5;
                }
            }

            UseAction();
        }
    }

    public void OrganizeBookshelf()
    {
        if (CanAct())
        {
            ShowActionDialogue("(학급 문고를 정리했다.)");
            
            switch (currentDay)
            {
                case 1:
                    // 1일차: 계몽 수치 변경 없음
                    break;
                case 2:
                    // 2일차: 계몽 수치 +5
                    enlightenmentMeter += 5;
                    break;
                default:
                    // 예시: 그 외의 날: 30% 확률로 계몽 수치 +10
                    if (Random.Range(0, 100) < 30)
                    {
                        enlightenmentMeter += 10;
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
            ShowActionDialogue("(식물에 물을 주었다.)");
            
            // 1일차에는 계몽 수치 변경 없음
            if (currentDay > 1)
            {
                if (currentDay == 2)
                {
                    enlightenmentMeter -= 5; // 2일차
                }
                else
                {
                    enlightenmentMeter -= 3; 
                }
            }
            
            UseAction();
        }
    }

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
            ShowActionDialogue("(주번 활동을 끝냈다...)");
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

    public void LoadData(GameData data)
    {
        if (data == null)
        {
            Debug.LogError("GameData is null. Cannot load data.");
            return;
        }
        this.currentDay = data.currentDay;
        this.actionsLeft = data.actionsLeft;
        this.enlightenmentMeter = data.enlightenmentMeter;
        this.girlFavorability = data.girlFavorability;
        this.glassesFavorability = data.glassesFavorability;
        this.rabbitFavorability = data.rabbitFavorability;
        this.interactedObjectIds = new HashSet<string>(data.interactedObjectIds);
        Debug.LogWarning($"[GameManager] LoadData가 호출되었습니다. actionsLeft가 <color=yellow>{this.actionsLeft}</color>로 설정됩니다.");
    }

    public void SaveData(GameData data)
    {
        if (data == null)
        {
            Debug.LogError("GameData is null. Cannot save data.");
            return;
        }
        
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentSceneName != "MainMenuScene")
        {
            data.lastSceneName = currentSceneName; 
        }
        data.currentDay = this.currentDay;
        data.actionsLeft = this.actionsLeft;
        data.enlightenmentMeter = this.enlightenmentMeter;
        data.girlFavorability = this.girlFavorability;
        data.glassesFavorability = this.glassesFavorability;
        data.rabbitFavorability = this.rabbitFavorability;
        data.interactedObjectIds = new List<string>(this.interactedObjectIds);
        Debug.Log("Saved game data");
    }
}