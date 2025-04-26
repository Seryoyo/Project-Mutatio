using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    /*public Sprite emptyChest;*/
    /*public int pesosAmount = 10;*/
    public string itemName;
    public int itemQuantity;


    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GameManager.instance.GrantItem(itemName, itemQuantity);
            // GetComponent <SpriteRenderer>().sprite = emptyChest; //
            // GameManager.instance.ShowText("+" + pesosAmount + " pesos", 25, Color.yellow, transform.position, Vector3.up * 50, 3.0f);
            if (GetComponent<SpriteRenderer>() != null)
                GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
