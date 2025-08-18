using UnityEngine;

public class TeacherController : MonoBehaviour
{
    // Inspector 창에서 제어할 실제 선생님 게임 오브젝트
    public GameObject teacherObject;

    // 선생님의 애니메이터 컴포넌트
    private Animator anim;

    void Start()
    {
        // teacherObject 또는 이 스크립트가 붙어있는 오브젝트에서 Animator 컴포넌트 찾기
        // 자식 오브젝트에 Animator가 있을 수도 있으니 GetComponentInChildren 사용
        anim = GetComponentInChildren<Animator>();
        
        if (GameManager.instance != null)
        {
            GameManager.instance.onStateChanged.AddListener(CheckActionsLeft);
            CheckActionsLeft(); 
        }
        else
        {
            Debug.LogError("GameManager를 찾을 수 없음. 씬에 GameManager가 있는지 확인 필요");
        }
    }

    private void CheckActionsLeft()
    {
        if (teacherObject == null)
        {
            Debug.LogError("[TeacherController] Teacher Object 변수가 빔. Inspector에서 설정 필요.");
            return;
        }

        bool isActive = GameManager.instance.actionsLeft > 0;
        teacherObject.SetActive(isActive);

        if(isActive)
        {
            Debug.Log($"[TeacherController] 행동 횟수가 {GameManager.instance.actionsLeft}이므로 선생님 오브젝트를 활성화");
        }
        else
        {
            Debug.Log($"[TeacherController] 행동 횟수가 0 이하이므로 선생님 오브젝트를 비활성화");
        }
    }
    
    // 플레이어가 감지 범위(트리거) 안으로 들어왔을 때 호출
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 들어온 오브젝트의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지! 보는 방향을 변경");
            // 애니메이터의 "PlayerIsNear" 파라미터를 true로 설정
            if (anim != null)
            {
                anim.SetBool("PlayerIsNear", true);
            }
        }
    }

    // 플레이어가 감지 범위(트리거) 밖으로 나갔을 때 호출
    private void OnTriggerExit2D(Collider2D other)
    {
        // 나간 오브젝트의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 벗어남, 원래 상태로 돌아감");
            // 애니메이터의 "PlayerIsNear" 파라미터를 false로 설정
            if (anim != null)
            {
                anim.SetBool("PlayerIsNear", false);
            }
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onStateChanged.RemoveListener(CheckActionsLeft);
        }
    }
}