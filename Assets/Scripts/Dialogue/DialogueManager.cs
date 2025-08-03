using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public bool isDialogueActive { get; private set; } = false;
    public float dialogueSpeed = 0.05f; // Speed of dialogue text display
    public Animator animator; // Animator for dialogue UI
    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this object persists across scenes
        }
        else
        {
            Destroy(gameObject); // If an instance already exists, destroy this one
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        animator.Play("DialogueBoxPopUp");  // Play popup animation

        dialogueQueue.Clear();
        foreach (DialogueLine line in dialogue.dialogueLines)
        {
            dialogueQueue.Enqueue(line); // Add each dialogue line to the queue
        }
        DisplayNextLine();
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        animator.Play("DialogueBoxPopDown"); // Play dropdown animation
    }

    public void DisplayNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueQueue.Dequeue();

        line.onLineStart?.Invoke();     // Invoke the event for the line start

        characterName.text = line.characterName;
        
        StopAllCoroutines(); // Stop any ongoing typing effec
        StartCoroutine(TypeDialogue(line)); // Start typing effect
    }

    IEnumerator TypeDialogue(DialogueLine line)
    {
        dialogueText.text = line.dialogueText;
        dialogueText.maxVisibleCharacters = 0;

        yield return new WaitForSeconds(dialogueSpeed);

        int totalVisibleCharacters = dialogueText.textInfo.characterCount;
        int counter = 0;

        while(counter <= totalVisibleCharacters)
        {
            dialogueText.maxVisibleCharacters = counter;
            counter++;
            yield return new WaitForSeconds(dialogueSpeed);
        }
    }
}
