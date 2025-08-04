using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public GameObject[] interactionIcons;
    public UnityEvent onInteract;
    private bool isInteracted = false;

    void Start()
    {
        // 게임 시작 시 모든 아이콘을 숨김
        HideIcons();
    }

    public void Interact()
    {
        // 아직 상호작용하지 않았을 때만 실행
        if (!isInteracted)
        {
            onInteract.Invoke();
            isInteracted = true; // 상호작용했다고 표시
            HideIcons();         // 상호작용 후 아이콘을 영구적으로 숨기기
        }
    }

    // PlayerMovement가 호출할 아이콘 표시 함수
    public void ShowIcons()
    {
        // 상호작용하지 않은 오브젝트일 경우에만 아이콘을 표시
        if (!isInteracted)
        {
            foreach (GameObject icon in interactionIcons)
            {
                if (icon != null)
                {
                    icon.SetActive(true);
                }
            }
        }
    }

    // PlayerMovement가 호출할 아이콘 숨기기 함수
    public void HideIcons()
    {
        foreach (GameObject icon in interactionIcons)
        {
            if (icon != null)
            {
                icon.SetActive(false);
            }
        }
    }
}