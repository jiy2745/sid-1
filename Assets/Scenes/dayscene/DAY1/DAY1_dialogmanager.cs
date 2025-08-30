using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class day1_dialogmanager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Settings")]
    public float dialogueSpeed = 0.05f;

    public bool isDialogueActive { get; private set; } = false;
    
    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    private Action onDialogueCompleted;
    private bool isTyping = false;
    private string currentSentence;
    
 
    private bool acceptInput = false;

    void Start()
    {
        if (animator == null || characterNameText == null || dialogueText == null)
        {
            Debug.LogError("day1_dialogmanager의 UI 참조가 Inspector에 연결되지 않았습니다!", this.gameObject);
        }
    }

    void Update()
    {
        
        if (isDialogueActive && acceptInput && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentSentence;
                isTyping = false;
                acceptInput = true; 
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue, Action onCompletedCallback = null)
    {
        if (!isDialogueActive && animator != null)
        {
            
            acceptInput = false;

            animator.gameObject.SetActive(true);
            isDialogueActive = true;
            animator.Play("DialogueBoxPopUp");

            dialogueQueue.Clear();
            foreach (DialogueLine line in dialogue.dialogueLines)
            {
                dialogueQueue.Enqueue(line);
            }

            this.onDialogueCompleted = onCompletedCallback;
            DisplayNextLine();
        }
    }

    public void DisplayNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            StartCoroutine(EndDialogueRoutine());
            return;
        }

        acceptInput = false; 
        DialogueLine currentLine = dialogueQueue.Dequeue();
        currentSentence = currentLine.dialogueText; 
        characterNameText.text = currentLine.characterName;
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(currentSentence));
    }
    
    private IEnumerator TypeDialogue(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueSpeed);
        }
        isTyping = false;

       
        acceptInput = true;
    }
    
    private IEnumerator EndDialogueRoutine()
    {
        isDialogueActive = false;
        if (animator != null)
        {
            animator.Play("DialogueBoxPopDown");
            yield return new WaitForSeconds(0.5f);
            animator.gameObject.SetActive(false);
        }
        onDialogueCompleted?.Invoke();
        onDialogueCompleted = null;
    }
}