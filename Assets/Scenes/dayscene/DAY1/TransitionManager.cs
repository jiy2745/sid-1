using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime = 1f;

    
    private bool isInitialLoad = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (isInitialLoad)
        {
            isInitialLoad = false; 
            return;
        }

        
        animator.SetTrigger("StartFadeIn");
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        
        animator.SetTrigger("StartFadeOut");

        
        yield return new WaitForSeconds(transitionTime);

        
        SceneManager.LoadScene(sceneName);
    }
}