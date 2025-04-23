using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    protected string speakerName;
    protected string nextDialogue;

    protected void Start()
    {
        speakerName = "";
        nextDialogue = "";
    }

    // todo... maybe
        // disable movement while textbox is on
        // allow disabling an open textbox no matter where the user is
    public virtual void ToggleTextbox()
    {
        SetNextDialogue();
        GameManager.instance.ToggleDialogue(speakerName, nextDialogue);
    }

    protected virtual void SetNextDialogue()
    {

    }


}
