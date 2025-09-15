using UnityEngine;

public class CharacterLook : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public Sprite defaultSprite;          
    public Sprite lookAtEachOtherSprite;  

    void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

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