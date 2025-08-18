using TMPro;
using UnityEngine;

// Helper class that holds references to the dialogue UI components
// This allows the DialogueManager to find and use the UI elements when a new scene is loaded
public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
}
