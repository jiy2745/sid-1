using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        // 이 스크립트를 가진 오브젝트를 모두 찾는다. 
        GameObject[] objs = GameObject.FindGameObjectsWithTag(gameObject.tag);

        // 만약 나 말고도 이미 다른 Player 오브젝트가 있다면
        if (objs.Length > 1)
        {
            // 새로 생긴 나 자신을 파괴(충돌방지)
            Destroy(gameObject);
        }

        // 그렇지 않다면 (내가 첫 번째 Player라면)
        // 씬이 바뀌어도 나를 파괴하지 않도록 설정
        DontDestroyOnLoad(gameObject);
    }
}