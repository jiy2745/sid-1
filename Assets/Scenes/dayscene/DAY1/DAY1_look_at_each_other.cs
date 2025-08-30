using UnityEngine;

public class CharacterLook : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // 캐릭터의 SpriteRenderer 컴포넌트를 할당
    public Sprite defaultSprite;          // 평상시 스프라이트
    public Sprite lookAtEachOtherSprite;  // 서로를 바라볼 때의 스프라이트

    void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    // 외부에서 호출하여 스프라이트를 변경하는 함수
    public void SetLookSprite(bool lookingAtEachOther)
    {
        if (lookingAtEachOther)
        {
            spriteRenderer.sprite = lookAtEachOtherSprite;
        }
        else
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }
}