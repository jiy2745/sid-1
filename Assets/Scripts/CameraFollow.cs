using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform을 연결할 변수

    // 카메라가 멈춰야 할 경계선 좌표
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // 새로 추가: 씬 시작 시 플레이어를 자동으로 찾는다
    void Start()
    {
        // Inspector 창에서 Target이 미리 연결되지 않았다면
        if (target == null)
        {
            // "Player" 태그를 가진 오브젝트를 씬에서 찾기
            GameObject playerObject = GameObject.FindWithTag("Player");

            // 성공적으로 찾았다면
            if (playerObject != null)
            {
                // 해당 오브젝트의 Transform을 target으로 설정
                target = playerObject.transform;
            }
            else
            {
                Debug.LogError("카메라가 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다!");
            }
        }
    }

    // 모든 update가 끝난 후 마지막으로 수행되어야 하는 카메라 움직임
    void LateUpdate()
    {
        if (target == null)
        {
            return; // 타겟이 없으면 아무것도 안함
        }

        // 카메라의 새 위치를 계산 (플레이어의 위치를 따라가되, 카메라의 z축 위치는 유지)
        Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // 카메라의 x, y 위치를 정해진 경계값 안으로 제한
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // 계산된 최종 위치로 카메라를 이동
        transform.position = newPosition;
    }
}