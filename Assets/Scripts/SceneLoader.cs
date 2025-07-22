using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoader : MonoBehaviour
{
   //TrigerArea 에서 public으로 불러올 수 있는 SceneLoader 스크립트
    public void LoadSceneByName(string sceneName)
    {
        // sceneName이 비어있지 않다면 해당 씬을 불러온다.
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // 혹시 이름이 잘못 입력되었을 경우를 대비한 경고 메시지
            Debug.LogWarning("로딩할 씬 이름이 지정되지 않았습니다!");
        }
    }
}