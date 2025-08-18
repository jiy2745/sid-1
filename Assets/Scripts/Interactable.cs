using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Tooltip("이 오브젝트를 식별할 고유한 ID를 입력하세요.")]
    public string interactableId; // 각 오브젝트를 구분할 고유 ID

    public GameObject[] interactionIcons;
    public UnityEvent onInteract;

    
    void Start()
    {
        
        if (string.IsNullOrEmpty(interactableId))
        {
            Debug.LogError(gameObject.name + " 오브젝트에 Interactable ID가 설정되지 않았습니다! Inspector 창에서 ID를 입력해주세요.");
        }
        
        HideIcons();
    }

    public void Interact()
    {
        
       

        if (!GameManager.instance.IsInteracted(interactableId))
        {
            onInteract.Invoke();
            
           
            GameManager.instance.SetInteracted(interactableId); 
            
            HideIcons();
        }
    }

    public void ShowIcons()
    {
        
        if (!GameManager.instance.IsInteracted(interactableId))
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