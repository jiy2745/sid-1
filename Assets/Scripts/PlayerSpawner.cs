using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
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
        // "PlayerStartPoint" 태그를 가진 오브젝트 찾기 (DayScene_hallway 오브젝트로 추가해놨습니다)
        GameObject startPoint = GameObject.FindWithTag("PlayerStartPoint");

        // 만약 시작 지점을 찾았다면
        if (startPoint != null)
        {
            // 플레이어 (이 스크립트가 붙어있는 오브젝트) 의 위치를
            // 시작 지점의 위치로 옮기기.
            transform.position = startPoint.transform.position;
        }
    }
}