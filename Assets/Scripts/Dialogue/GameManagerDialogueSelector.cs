using System.Collections.Generic;
using UnityEngine;

// This class selects the appropriate dialogue based on the enlightment meter in the game manager.
// Intended for use on the game manager object for game-over dialogues
public class GameManagerDialogueSelector : DialogueSelector
{
    [System.Serializable]
    public class DialogueReferencebyEnlightenment
    {
        [Header("Conditions")]
        [Tooltip("Required conditions for this dialogue to trigger. Set to -1 to ignore.")]
        public int minEnlightenment; // Minimum enlightenment required for this dialogue to trigger
        public int maxEnlightenment; // Maximum enlightenment for this dialogue to trigger
        public DialogueReference dialogueReference; // The dialogue reference to use for this day
    }

    public DialogueReference defaultDialogue; // Default dialogue to use if no conditions are met

    public List<DialogueReferencebyEnlightenment> dialogues = new List<DialogueReferencebyEnlightenment>();

    public override DialogueReference SelectDialogue()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance is not available. Returning default dialogue.");
            return defaultDialogue;
        }
        foreach (var dialogue in dialogues)
        {
            int currentValue = GameManager.instance.enlightenmentMeter;
            bool minOk = dialogue.minEnlightenment < 0 || currentValue >= dialogue.minEnlightenment;
            bool maxOk = dialogue.maxEnlightenment < 0 || currentValue <= dialogue.maxEnlightenment;

            if (minOk && maxOk)
            {
                return dialogue.dialogueReference;
            }
        }
        return defaultDialogue; // Return default dialogue if no conditions are met
    }

}
