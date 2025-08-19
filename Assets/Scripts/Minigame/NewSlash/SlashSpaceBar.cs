using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlashSpaceBar : MonoBehaviour
{
    private Image spaceBarImage;
    public Sprite spaceBarPressed;
    public Sprite spaceBarLifted;
    public float pressInterval = 1f;

    private bool isPressed = false;
    private Coroutine pulseCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spaceBarImage = GetComponent<Image>();
    }

    void OnEnable()
    {
        if (pulseCoroutine == null)
        {
            pulseCoroutine = StartCoroutine(AlternateSprites());
        }
    }

    void OnDisable()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(AlternateSprites());
            pulseCoroutine = null;
        }
    }

    private IEnumerator AlternateSprites()
    {
        while (true)
        {
            spaceBarImage.sprite = isPressed ? spaceBarPressed : spaceBarLifted;
            isPressed = !isPressed;
            yield return new WaitForSeconds(pressInterval);
        }
    }
}
