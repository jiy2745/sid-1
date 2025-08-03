using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(3, 10)]
    public string dialogueText;
    public UnityEvent onLineStart; // Assign events in the inspector to trigger when the line starts
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

[System.Serializable]
public class ConditionalDialogue
{
    public string dialogueId; // Unique identifier for the dialogue (the file name without extension)

    [Header("Conditions")]
    [Tooltip("The day on which this dialogue can be triggered. Set to 0 for any day.")]
    public int requiredDay = 0;

    [Tooltip("Link UnityEvents here. Number of lines should match the dialogue file.")]
    public Dialogue dialogueEvents;
}

public class DialogueTrigger : MonoBehaviour
{
    public List<ConditionalDialogue> conditionalDialogues = new List<ConditionalDialogue>();

    public ConditionalDialogue defaultDialogue; // Default dialogue to trigger if no conditions are met

    public void TriggerDialogue()
    {
        ConditionalDialogue dialogueToPlay = GetActiveDialogue();
        if (dialogueToPlay == null || string.IsNullOrEmpty(dialogueToPlay.dialogueId))
        {
            Debug.LogWarning("No suitable dialogue found for this trigger.", this);
            return;
        }

        Dialogue loadedDialogue = DialogueLoader.LoadDialogue(dialogueToPlay.dialogueId);
        if (loadedDialogue != null && DialogueManager.instance != null)
        {
            // Combine loaded text with editor-defined events
            if (dialogueToPlay.dialogueEvents != null)
            {
                for (int i = 0; i < loadedDialogue.dialogueLines.Count; i++)
                {
                    if (i < dialogueToPlay.dialogueEvents.dialogueLines.Count)
                    {
                        loadedDialogue.dialogueLines[i].onLineStart = dialogueToPlay.dialogueEvents.dialogueLines[i].onLineStart;
                    }
                }
            }
            // Start the dialogue with the loaded dialogue lines
            DialogueManager.instance.StartDialogue(loadedDialogue);
        }
        else
        {
            Debug.LogError("Failed to load dialogue or DialogueManager instance is null.");
        }
    }

    private ConditionalDialogue GetActiveDialogue()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance is null. Cannot check conditions for dialogue.");
            return defaultDialogue; // Return default dialogue if GameManager is not available
        }

        foreach (ConditionalDialogue conditionalDialogue in conditionalDialogues)
        {
            if (conditionalDialogue.requiredDay == 0 || GameManager.instance.currentDay == conditionalDialogue.requiredDay)
            {
                return conditionalDialogue; // Return the first matching dialogue based on conditions
            }
        }
        return defaultDialogue; // Return default dialogue if no conditions are met
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue(); // Trigger dialogue when player enters the trigger area
        }
    }
}

public class DialogueLoader
{
    public static Dialogue LoadDialogue(string dialogueId)
    {
        // Construct the path within the Resources folder
        string path = "Dialogues/" + dialogueId;
        TextAsset jsonFile = Resources.Load<TextAsset>(path);

        if (jsonFile != null)
        {
            // Deserialize the JSON file into a Dialogue object
            return JsonUtility.FromJson<Dialogue>(jsonFile.text);
        }
        else
        {
            Debug.LogError($"Dialogue file not found at: {path}");
            return null;
        }
    }
}
