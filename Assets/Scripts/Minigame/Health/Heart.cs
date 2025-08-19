using UnityEngine;
using UnityEngine.UI;
public class Heart : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private Image heartImage;
    private Animator animator;

    void Awake()
    {
        heartImage = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    public void SetFull()
    {
        heartImage.sprite = fullHeart;
    }

    public void SetEmpty()
    {
        heartImage.sprite = emptyHeart;
    }

    public void PlayEmptyAnimation()
    {
        animator.Play("health_decrease");
    }

    public void PlayFillAnimation()
    {
        animator.Play("health_increase");
    }
}
