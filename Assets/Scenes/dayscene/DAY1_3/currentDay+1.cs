using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class DayTransitionController : MonoBehaviour
{
    [Header("전환할 씬 설정")]
    [Tooltip("다음 날에 불러올 씬의 정확한 이름을 입력하세요.")]
    public string nextDaySceneName;

    public void TriggerNextDay()
    {
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        Debug.Log("하루를 마감하고 다음 날로 전환을 시작합니다...");

 
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다! 날짜를 변경할 수 없습니다.");
            yield break; 
        }

        
        if (string.IsNullOrEmpty(nextDaySceneName))
        {
            Debug.LogError("전환할 씬 이름(Next Day Scene Name)이 지정되지 않았습니다!");
            yield break;
        }

  
        GameManager.instance.StartNewDay();

     
        Debug.Log($"'{nextDaySceneName}' 씬으로 전환합니다. 새로운 날이 시작됩니다!");
        SceneManager.LoadScene(nextDaySceneName);
    }
}