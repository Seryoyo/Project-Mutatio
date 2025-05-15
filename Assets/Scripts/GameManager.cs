using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Resources
    public Dictionary<string, int> inventory;

    // References
    public Player player;
    public FloatingTextManager floatingTextManager;
    public Inventory inventoryMenu;
    public DialogueWindow dialogueWindow;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // game start state
        instance = this;
        DontDestroyOnLoad(gameObject);
        inventory = new Dictionary<string, int>()
        {
            // zero out before publishing OK!!!
            ["Band-Aid"] = 1,
            ["Regeneration Tablet"] = 1,
            ["Neural Stabilizer"] = 1,
            ["Psy-Delimiter"] = 1,
            ["Cell Fortifier"] = 1,
            ["Inertia Suppressant"] = 1,
        };
    }

    // Floating text
    public void ShowText(string msg, int fontSize, UnityEngine.Color color, Vector3 position, Vector3 motion, float duration, bool WorldToScreenSpace = true)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration, WorldToScreenSpace);
    }

    public void ToggleDialogue(string name = "", string msg = "")
    {
        dialogueWindow.Toggle(name, msg);
    }

    public void HideDialog()
    {
        dialogueWindow.Hide();
    }

    public void GrantItem(string name, int quantity)
    {
        if (quantity != 0) { 
            inventory[name] += quantity;
            floatingTextManager.Show($"Obtained {quantity} {name}{((quantity>1)?"s":"")}!",
                                        30, UnityEngine.Color.green, 
                                        player.transform.position,
                                        new Vector3 (0, 50f, 0),
                                        2f);
            inventoryMenu.UpdateInventory();
        }
    }

    public void TakeItem(string name, int quantity, bool used = false)
    {
        if (quantity != 0) { 
            inventory[name] -= quantity;
            floatingTextManager.Show($"{(used ? "Used" : "Lost")}{((quantity==1)?"":quantity)} {name}{((quantity > 1) ? "s" : "")}.\n\n",
                                        30,
                                        UnityEngine.Color.yellow,
                                        player.transform.position,
                                        new Vector3(0, 50f, 0), 2f);
            inventoryMenu.UpdateInventory();
        }
    }

    public void CalculateRandomDrop()
    {
        var num = Random.Range(1, 100); // both bounds are inclusive
        switch (num)
        {
            case (> 50): return;            // 50% no item for u
            case (> 30):                    // 20% band-aid
                GrantItem("Band-Aid", 1);
                return;
            case (> 20):                    // 10% big heal
                GrantItem("Regeneration Tablet", 1);
                return;
            case (> 9):                     // 11% mut heal
                GrantItem("Neural Stabilizer", 1);
                return;
            default:                        // Mutation item
                GiveMutItem();
                return;
        }
    }

    // sum of mut level and corresponding mut items currently in inventory cannot exceed 3
    public void GiveMutItem()
    {
        var num = Random.Range(1, 3);
        switch (num)
        {
            case 1:     // Psy-Delimiter (Bullet)
                if ((player.bulletLevel + inventory["Psy-Delimiter"]) < 3)
                    GrantItem("Psy-Delimiter", 1);
                return;
            case 2:     // Cell Fortifier (Max health)
                if ((player.bulletLevel + inventory["Cell Fortifier"]) < 3)
                    GrantItem("Cell Fortifier", 1);
                return;
            case 3:     // Inertia Suppressant (Speed)
                if ((player.bulletLevel + inventory["Inertia Suppressant"]) < 3)
                    GrantItem("Inertia Suppresant", 1);
                return;
            default:
                Debug.Log("Invalid GiveMutItem calculation :(");
                return;
        }
    }

}