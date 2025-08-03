using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // (08/03 진성민)두 개의 파라미터를 받기: 씬 이름과 스폰 지점 이름
    public void LoadSceneWithSpawnPoint(string sceneName, string spawnName)
    {
        // PlayerSpawner에게 다음 스폰 지점의 이름을 알려주기
        PlayerSpawner.nextSpawnName = spawnName;

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("로딩할 씬 이름이 지정되지 않았습니다!");
        }
    }
}