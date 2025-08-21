using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup backgroundGroup;
    public CanvasGroup textGroup;
    public CanvasGroup buttonsGroup;

    public Button retryButton;
    public Button menuButton;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;      // How long each fade takes
    public float interval = 0.5f;        // Delay between each element

    private void Start()
    {
        // Ensure all groups start invisible
        SetAlpha(backgroundGroup, 0f);
        SetAlpha(textGroup, 0f);
        SetAlpha(buttonsGroup, 0f);

        gameObject.SetActive(false);

        retryButton.onClick.AddListener(() => OnButtonClicked("NightScene"));
        // menuButton.onClick.AddListener(() => OnButtonClicked("MainMenu")); // TODO: Uncomment when MainMenu scene is available
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowSequence());
    }

    private IEnumerator ShowSequence()
    {
        yield return new WaitForSeconds(1f); // optional delay before Game Over UI

        yield return StartCoroutine(FadeIn(backgroundGroup));
        yield return new WaitForSeconds(interval);

        yield return StartCoroutine(FadeIn(textGroup));
        yield return new WaitForSeconds(interval);

        yield return StartCoroutine(FadeIn(buttonsGroup));
    }

    private IEnumerator FadeIn(CanvasGroup group)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        group.alpha = 1f;
    }

    private void SetAlpha(CanvasGroup group, float value)
    {
        if (group != null)
            group.alpha = value;
    }

    private void OnButtonClicked(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        // Fade out everything together
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            SetAlpha(backgroundGroup, alpha);
            SetAlpha(textGroup, alpha);
            SetAlpha(buttonsGroup, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
