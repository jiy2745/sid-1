using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(DialogueTrigger))]
public abstract class DialogueSelector : MonoBehaviour
{
    public abstract DialogueReference SelectDialogue();
}
