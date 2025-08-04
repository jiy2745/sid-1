using System.Collections.Generic;
using UnityEngine;

// This class selects the appropriate dialogue based on the current day in the game.
// Intended for use with NPCs that have dialogues that change based on the day
public class NPCDialogueSelector : DialogueSelector
{
    [System.Serializable]
    public class DialogueReferencebyDay
    {
        [Header("Conditions")]
        [Tooltip("Required day for this dialogue to trigger. Set to 0 to ignore.")]
        public int requiredDay; // The day on which this dialogue can be triggered
        public DialogueReference dialogueReference; // The dialogue reference to use for this day
    }
    
    public int requiredDay = 0; // The day on which this dialogue can be triggered. Set to 0 for any day.

    public DialogueReference defaultDialogue; // Default dialogue to use if no conditions are met

    public List<DialogueReferencebyDay> dialogues = new List<DialogueReferencebyDay>();

    public override DialogueReference SelectDialogue()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance is not available. Returning default dialogue.");
            return defaultDialogue;
        }
        foreach (var dialogue in dialogues)
        {
            if (dialogue.requiredDay <= 0 || GameManager.instance.currentDay == dialogue.requiredDay)
            {
                return dialogue.dialogueReference; // Return the first matching dialogue based on conditions
            }
        }
        return defaultDialogue; // Return default dialogue if no conditions are met
    }

}
