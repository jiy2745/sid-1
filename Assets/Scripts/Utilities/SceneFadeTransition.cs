using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class SceneFadeTransition : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;         // Fullscreen UI Image (black, stretched to cover screen)
    [SerializeField] private float fadeDuration = 1f; // Seconds for fade

    private void Awake()
    {
        if (fadeImage != null)
        {
            // Ensure image starts transparent
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    /// <summary>
    /// Call this method via UnityEvent, passing the scene name you want to load.
    /// </summary>
    public void FadeOutAndLoadScene(string sceneName)
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeOutRoutine(sceneName));
        }
        else
        {
            Debug.LogError("SceneFader: No fadeImage assigned!");
        }
    }

    private IEnumerator FadeOutRoutine(string sceneName)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // Fully black now, load the scene
        SceneManager.LoadScene(sceneName);
    }
}
