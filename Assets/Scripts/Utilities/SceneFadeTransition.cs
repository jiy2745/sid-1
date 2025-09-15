using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// Helper class to call singleton SceneFade instance from other scripts
public class SceneFadeTransition : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (SceneFade.instance == null)
        {
            Debug.LogError("SceneFade instance not found!");
            return;
        }
        SceneFade.LoadScene(sceneName);
    }
}
