using UnityEngine;
using TMPro;

public class DebugCanvas : MonoBehaviour
{
    public MinigameManager manager;
    public TMP_Text currentMinigame;


    // Update is called once per frame
    void Update()
    {
        switch (manager.currentGame)
        {
            // No minigame is running, enable player movement
            case Minigame.NONE:
                currentMinigame.text = "Current Minigame : None";
                break;
            case Minigame.QTE_MINIGAME:
                currentMinigame.text = "Current Minigame : QTE";
                break;
            case Minigame.DODGE_MINIGAME:
                currentMinigame.text = "Current Minigame : Dodge";
                break;
            case Minigame.SLASH_MINIGAME:
                currentMinigame.text = "Current Minigame : Slash";
                break;
        }
    }
}
