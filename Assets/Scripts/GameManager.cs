using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Resources
    public string currentWeapon;
    /*public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<Sprite> itemSprites;*/
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
        //currentWeapon = "Tiller"; // Make "" to start w/ nothing. Using stronger weapon for debugging
        inventory = new Dictionary<string, int>()
        {
            ["Band-Aid"] = 1,
            ["Regeneration Tablet"] = 0,
            ["Psy-Delimiter"] = 1,
            ["Neural Stabilizer"] = 1,
            ["Milk"] = 0,
            ["Coffee"] = 0,
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

}