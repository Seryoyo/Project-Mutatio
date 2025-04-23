using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObject : NPC // This is such a stupid name but I don't know what else to call it
{
    public string FlavorText;

    public override void ToggleTextbox()
    {
        GameManager.instance.ToggleDialogue(msg: FlavorText);
    }
}
