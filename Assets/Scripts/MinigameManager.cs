using UnityEngine;

public enum Minigame
{
    NONE,
    QTE_MINIGAME
}

public class MinigameManager : MonoBehaviour
{
    public Minigame currentGame = Minigame.NONE;
    public QTEMinigame qteMinigame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGame = Minigame.QTE_MINIGAME;
        qteMinigame.enabled = true;
        //qteMinigame.StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
