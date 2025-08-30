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
public class DialogueReference
{
    public string dialogueId; // Unique identifier for the dialogue (the file name without extension)

    [Tooltip("Link UnityEvents here. Number of lines should match the dialogue file.")]
    public Dialogue dialogueEvents;
}

public class DialogueTrigger : MonoBehaviour
{
    private DialogueSelector dialogueSelector;

    [Header("Post-Dialogue Events")]
    [Tooltip("Event fired after dialogue sequence ends.")]
    public UnityEvent OnDialogueCompleted; // Event to call when dialogue ends

    private void Awake()
    {
        dialogueSelector = GetComponent<DialogueSelector>();
        if (dialogueSelector == null)
        {
            Debug.LogError("DialogueSelector component is missing on this GameObject.", this);
        }
    }
    public void TriggerDialogue()
    {
        DialogueReference dialogueToPlay = dialogueSelector.SelectDialogue();
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
            DialogueManager.instance.StartDialogue(loadedDialogue, OnDialogueCompleted.Invoke);
        }
        else
        {
            Debug.LogError("Failed to load dialogue or DialogueManager instance is null.");
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue(); // Trigger dialogue when player enters the trigger area
        }
    }
    */
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
