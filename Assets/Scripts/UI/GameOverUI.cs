using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Button retryButton;
    public Button menuButton;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Set to invisible at start
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);

        retryButton.onClick.AddListener(() => OnButtonClicked("NightScene"));
        //menuButton.onClick.AddListener(() => OnButtonClicked("MainMenu")); // TODO: Uncomment when MainMenu scene is available
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    private void OnButtonClicked(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
