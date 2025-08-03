using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    //(08/03 진성민)
    public static string nextSpawnName;
    void OnEnable()
    {
        // 씬이 로드되었을 때 OnSceneLoaded 함수를 실행하도록 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // 비활성화될 때 등록을 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 로드되었을 때 자동으로 호출되는 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    // 만약 지정된 스폰 지점 이름이 있다면
        if (!string.IsNullOrEmpty(nextSpawnName))
        {
            // 씬에 있는 모든 SpawnPoint를 찾는다
            SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
            foreach (SpawnPoint point in spawnPoints)
            {
                // 이름이 일치하는 스폰 지점을 찾았다면
                if (point.spawnName == nextSpawnName)
                {
                    // 플레이어의 위치를 해당 지점의 위치로 옮긴다
                    transform.position = point.transform.position;
                    break; // 찾았으니 반복을 멈춤
                }
            }
            // 위치를 옮긴 후에는, 다음 씬 이동을 위해 이름을 비운다
            nextSpawnName = null;
        }
    }
}