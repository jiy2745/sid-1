using System.Collections.Generic;
using UnityEngine;

// This class is the basic implementation of a dialogue selector.
// It only holds a single dialogue reference, used for simple cases where no conditions are needed.
public class BasicDialogueSelector : DialogueSelector
{
    public DialogueReference Dialogue;

    public override DialogueReference SelectDialogue()
    {
        return Dialogue;
    }

}
