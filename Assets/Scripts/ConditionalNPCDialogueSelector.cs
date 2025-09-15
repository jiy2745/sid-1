using System.Collections.Generic;
using UnityEngine;

// 날짜, 계몽수치, 호감도 등 다양한 조건을 복합적으로 판단하여 대사를 선택하는 클래스
public class ConditionalNPCDialogueSelector : DialogueSelector
{
    [System.Serializable]
    public class DialogueRule
    {
        [Tooltip("규칙의 우선순위를 결정합니다. 숫자가 높은 규칙부터 먼저 검사합니다.")]
        public int priority = 0; // 우선순위 (높은 숫자 먼저 체크)

        [Header("조건 (Condition)")]
        [Tooltip("이 규칙이 적용될 날짜. 0 이하는 모든 날짜에 적용.")]
        public int requiredDay = 0;
        
        [Tooltip("필요한 최소 계몽 수치. -1은 무시.")]
        public int minEnlightenment = -1;
        [Tooltip("허용되는 최대 계몽 수치. -1은 무시.")]
        public int maxEnlightenment = -1;

        [Tooltip("필요한 최소 '소녀' 호감도. -1은 무시.")]
        public int minGirlFavorability = -1;
        // (필요하다면 다른 NPC 호감도 조건도 여기에 추가할 수 있습니다)

        [Header("결과 (Result)")]
        public DialogueReference dialogueReference;
    }

    [Tooltip("위의 어떤 규칙에도 해당하지 않을 경우 출력될 기본 대사")]
    public DialogueReference defaultDialogue;
    [Tooltip("대사를 결정할 규칙 목록입니다. 우선순위(Priority)가 높은 순서대로 검사합니다.")]
    public List<DialogueRule> dialogueRules = new List<DialogueRule>();

    public override DialogueReference SelectDialogue()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다.");
            return defaultDialogue;
        }
        
        // 우선순위에 따라 규칙을 정렬 (내림차순, 높은 숫자가 먼저 오도록)
        dialogueRules.Sort((a, b) => b.priority.CompareTo(a.priority));

        int currentDay = GameManager.instance.currentDay;
        int currentEnlightenment = GameManager.instance.enlightenmentMeter;
        int currentGirlFavorability = GameManager.instance.girlFavorability;

        // 정렬된 규칙을 하나씩 확인
        foreach (var rule in dialogueRules)
        {
            // 날짜 조건 확인
            bool dayMatch = (rule.requiredDay <= 0 || currentDay == rule.requiredDay);
            // 계몽 수치 조건 확인
            bool enlightenmentMatch = (rule.minEnlightenment < 0 || currentEnlightenment >= rule.minEnlightenment) &&
                                      (rule.maxEnlightenment < 0 || currentEnlightenment <= rule.maxEnlightenment);
            // 호감도 조건 확인
            bool favorabilityMatch = (rule.minGirlFavorability < 0 || currentGirlFavorability >= rule.minGirlFavorability);

            // 모든 조건이 일치하면 해당 대사를 반환
            if (dayMatch && enlightenmentMatch && favorabilityMatch)
            {
                return rule.dialogueReference;
            }
        }

        // 모든 규칙에 맞지 않으면 기본 대사 반환
        return defaultDialogue;
    }
}