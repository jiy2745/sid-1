using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    public string nextPlayerSpawnPointName;

    [Header("게임 상태 변수")]
    public int currentDay = 1;
    public int actionsLeft = 6;
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

    private void ShowActionDialogue(Dialogue dialogue)
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

        dialogueManager.StartDialogue(dialogue, null, true);
    }

    public void WriteJournal()
    {
        if (CanAct())
        {
            ShowActionDialogue("(오늘 있던 일을 되돌아보며 학급 일지를 작성했다.)");

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
            switch (currentDay)
            {
                case 1:
                    girlFavorability++;
                    ShowActionDialogue("주번 활동을 하고 있구나? 멋있어!", "레이나");
                    break;
                case 2:
                    Dialogue dialogue = new Dialogue();
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "아침부터 표정이 많이 안 좋던데, 무슨 일 있어?" });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "안 좋은 꿈이라도 꾼거야?" });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 적당히 얼버무렸다)" });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "그렇구나. 하긴, 가끔 현실이랑 구분이 안가는 악몽을 꾸고 나면 하루 종일 우울해질 때가 있긴 하지." });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "그럴 때는 행복한 생각을 하면 되지 않을까?" });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "행복했던 일을 떠올리고, 행복한 일을 하면서 행복한 현실을 바라보는 거야." });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "레이나", dialogueText = "행복한 이곳이야말로 진짜 현실이라는 걸, 온 몸으로 깨달으면 되는 거 아니겠어?" });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(그녀는 그렇게 말하며 환하게 웃었다)" });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 어쩌면 그녀의 조언이 맞을지도 모르겠다는 생각이 들었다.)" });

                    ShowActionDialogue(dialogue);

                    girlFavorability++;
                    enlightenmentMeter -= 8;
                    break;
                default:
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
            switch (currentDay)
            {
                case 2:
                    Dialogue dialogue = new Dialogue();
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(토끼에게 밥을 주려고 하던 그 순간, 토끼들의 등 뒤에 또 하나의 눈이 있는 걸 발견했다)" });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(상식을 벗어난 상황에, 나도 모르게 식은 땀이 흘렀다)" });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(나는 토끼에게 얼른 밥을 주고 자리를 떴다)" });
                    ShowActionDialogue(dialogue);
                    enlightenmentMeter += 5;
                    break;

                default:
                    ShowActionDialogue("(토끼에게 밥을 주었다.)");
                    break;
            }

            rabbitFavorability++;
            UseAction();
        }
    }

    public void SweepFloor()
    {
        if (CanAct())
        {
            switch (currentDay)
            {
                case 2:
                    Dialogue dialogue = new Dialogue();
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(교실 바닥을 열심히 쓸었다)" });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(교실을 조용히 쓸고 있다보니, 정신이 맑아지는 듯 했다)" });
                    ShowActionDialogue(dialogue);
                    enlightenmentMeter -= 5;
                    break;

                default:
                    ShowActionDialogue("(교실 바닥을 열심히 쓸었다.)");
                    if (currentDay > 1)
                    {
                        enlightenmentMeter -= 3;
                    }
                    break;
            }
            UseAction();
        }
    }

    public void CollectHomework()
    {
        if (CanAct())
        {
            ShowActionDialogue("(학생들이 제출한 과제를 순서대로 정리했다.)");

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
            switch (currentDay)
            {
                case 2:
                    Dialogue dialogue = new Dialogue();
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "학급 문고를 정리하다가, 학급 문고 안에 알 수 없는 언어로 적힌 책이 몇 개 있는 것을 발견했다." });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(분명 어제도 이런 책들이 있었던 것 같은데, 왜 그때는 이상하게 느껴지지 않았지…?)" });
                    dialogue.dialogueLines.Add(new DialogueLine { characterName = "나", dialogueText = "(온 몸에 소름이 돋았다)" });
                    ShowActionDialogue(dialogue);
                    enlightenmentMeter += 5;
                    break;

                default:
                    ShowActionDialogue("(학급 문고를 정리했다.)");
                    if (currentDay > 2)
                    {
                        if (Random.Range(0, 100) < 30)
                        {
                            enlightenmentMeter += 10;
                        }
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
            if (currentDay == 2)
            {
                ShowActionDialogue("(식물에 물을 주었다. 물이 졸졸 흐르는 소리를 들으며 가만히 식물을 보고 있으니 기분이 편안해졌다.)");
            }
            else
            {
                ShowActionDialogue("(식물에 물을 주었다.)");
            }

            if (currentDay > 1)
            {
                if (currentDay == 2)
                {
                    enlightenmentMeter -= 5;
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

            if (currentDay == 2)
            {
                StartCoroutine(ShowEndOfDayDialogueRoutine());
            }
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
        interactedObjectIds.Clear();
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

  
    private System.Collections.IEnumerator ShowEndOfDayDialogueRoutine()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<day1_dialogmanager>();
        }

        yield return new WaitUntil(() => dialogueManager != null && !dialogueManager.isDialogueActive);
        
        yield return new WaitForSeconds(0.1f);
        
        ShowActionDialogue("(어느새 기숙사로 돌아갈 시간이 되었다)");
    }
}