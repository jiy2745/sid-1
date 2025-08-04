using UnityEngine;

public class GameManagerActions : MonoBehaviour
{
    // 이 스크립트는 GameManager의 함수들을 대신 호출해주는 역할만 한다

    public void CallFeedRabbit()
    {
        // 싱글턴 인스턴스를 통해 어떤 씬에 있든 GameManager를 찾아 함수를 호출
        if (GameManager.instance != null)
        {
            GameManager.instance.FeedRabbit();
        }
    }

    public void CallWriteJournal()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.WriteJournal();
        }
    }

    public void CallWaterSprout()
{
    if (GameManager.instance != null)
    {
        GameManager.instance.WaterSprout();
    }
}

    // 이후 여기에 다른 GameManager 함수를 호출하는 public 함수들을 추가가능
}