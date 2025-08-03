using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [Header("이동할 씬 정보")]
    public string sceneToLoad;
    public string spawnPointName;

    // 이 함수를 호출하면 씬을 이동시킴
    public void TransitionToScene()
    {
        // SceneLoader를 찾아 씬 이동을 요청
        FindObjectOfType<SceneLoader>().LoadSceneWithSpawnPoint(sceneToLoad, spawnPointName);
    }
}