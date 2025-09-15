using System.Collections.Generic;
using UnityEngine;

// NPCDialogueSelector를 기반으로, 날짜별로 여러 대사를 리스트에 담아두고 그 중 하나를 랜덤으로 선택
public class RandomNPCDialogueSelector : DialogueSelector
{
    [System.Serializable]
    public class DailyRandomDialogues
    {
        [Tooltip("이 대사 목록을 사용할 날짜(Day)")]
        public int day;
        [Tooltip("이 날짜에 출력될 랜덤 대사 목록")]
        public List<DialogueReference> dialogues; // 해당 날짜에 사용할 대사 목록
    }

    [Tooltip("조건에 맞는 날짜가 없을 경우 출력될 기본 대사")]
    public DialogueReference defaultDialogue;
    [Tooltip("날짜별로 랜덤 대사 목록을 설정합니다.")]
    public List<DailyRandomDialogues> dailyDialogues = new List<DailyRandomDialogues>();

    public override DialogueReference SelectDialogue()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다.");
            return defaultDialogue;
        }

        int currentDay = GameManager.instance.currentDay;

        // 현재 날짜와 일치하는 대사 목록 탐색
        foreach (var dailySet in dailyDialogues)
        {
            if (dailySet.day == currentDay)
            {
                // 해당 날짜의 대사 목록이 비어있지 않다면
                if (dailySet.dialogues != null && dailySet.dialogues.Count > 0)
                {
                    // 목록 내에서 랜덤한 인덱스를 선택
                    int randomIndex = Random.Range(0, dailySet.dialogues.Count);
                    // 랜덤으로 선택된 대사를 반환
                    return dailySet.dialogues[randomIndex];
                }
            }
        }

        // 일치하는 날짜의 대사 목록이 없으면 기본 대사를 반환
        return defaultDialogue;
    }
}