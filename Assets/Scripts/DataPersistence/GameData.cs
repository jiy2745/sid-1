using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // 게임 상태 data
    public int currentDay;          // 현재 날짜
    public int currentTime;        // 현재 시간 (밤 / 낮)
    public int actionsLeft;         // 하루에 남은 행동 횟수
    public int enlightenmentMeter; // 계몽 수치

    // NPC 호감도 data
    public int girlFavorability;    // 옆자리 소녀 호감도
    public int glassesFavorability; // 안경 소녀 호감도
    public int rabbitFavorability;  // 토끼 호감도

    public List<string> interactedObjectIds;

    public GameData()
    {
        currentDay = 1;          // 현재 날짜
        actionsLeft = 4;         // 하루에 남은 행동 횟수
        enlightenmentMeter = 50; // 계몽 수치

        // NPC 호감도 data
        girlFavorability = 0;    // 옆자리 소녀 호감도
        glassesFavorability = 0; // 안경 소녀 호감도
        rabbitFavorability = 0;  // 토끼 호감도

        interactedObjectIds = new List<string>();

    }
}
