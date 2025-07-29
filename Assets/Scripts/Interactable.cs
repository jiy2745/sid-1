using UnityEngine;
using UnityEngine.Events; 

public class Interactable : MonoBehaviour
{
    // 인스펙터 창에서 유니티 이벤트를 연결할 수 있도록 설정
    public UnityEvent onInteract;

    // 해당 함수를 호출하면 연결된 모든 이벤트가 실행
    public void Interact()
    {
        onInteract.Invoke();
    }
}