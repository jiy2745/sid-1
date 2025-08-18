using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class DamageFlash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Flash Settings")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;   // how long the flash lasts
    public int flashCount = 2;           // how many times to flash

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void PlayDamageFlash()
    {
        StopAllCoroutines(); // stop previous flashes if overlapping
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
