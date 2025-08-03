using UnityEngine;

public class IconStateChecker : MonoBehaviour
{
    
    void OnEnable()
    {
       
        Debug.LogWarning("### 감시 스크립트 작동 ### " + gameObject.name + " 오브젝트가 방금 켜졌습니다!", gameObject);
        
        Debug.Log(System.Environment.StackTrace);
    }
}