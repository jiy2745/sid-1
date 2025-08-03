using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(3, 10)]
    public string dialogueText;
    public UnityEvent onLineStart; // Event to trigger when the line starts
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogError("DialogueManager instance is not set. Make sure it is initialized before triggering dialogue.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue(); // Trigger dialogue when player enters the trigger area
        }
    }
}
