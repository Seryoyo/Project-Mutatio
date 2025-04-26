using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    IItem selectedItem;
    ItemSlot[] slots;
    public Animator animator;
    private bool isShowing;
    public TextMeshProUGUI ItemTitle;
    public TextMeshProUGUI ItemDescription;
    public Button UseItemButton;
    public TextMeshProUGUI UseItemButtonText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        slots = GetComponentsInChildren<ItemSlot>();
        animator = GetComponent<Animator>();
        isShowing = false;
        UseItemButton.onClick.AddListener(OnUseButtonClicked);
        GetComponent<Canvas>().enabled = true;
    }

    private void Start()
    {
        GameManager.instance.inventoryMenu = this;
        UpdateItemDetails();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ToggleInventory();
    }

    public void ToggleInventory()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        // it works okay
        animator.ResetTrigger("show");
        animator.ResetTrigger("hide");
        if (!isShowing)
            animator.SetTrigger("show");
        else
            animator.SetTrigger("hide");

        isShowing = !isShowing;
    }

    public void UpdateInventory()
    {
        int slotsIndex = 0;
        foreach (var kv in GameManager.instance.inventory)
        {
            if (kv.Value > 0)
            {
                Debug.Log("Trying to map item name to IItem object");
                IItem entry = GetItemFromName(kv.Key);
                if (entry != null)
                {
                    Debug.Log("Entry name: " + entry.itemID);
                    slots[slotsIndex].item = entry;
                    slots[slotsIndex].UpdateInfo();
                    slotsIndex++;
                } else
                {
                    Debug.Log("Entry was null :( what the hell man");
                }
            }
        }
        while (slotsIndex < slots.Length) // Blank out all the unused slots
        {
            slots[slotsIndex].BlankOutSlot();
            slotsIndex++;
        }

        // TODO: f selected item is null or its value from gamemanager key <= 0, deselect..?
        selectedItem = ((slots[0].item != null) ? slots[0].item : null); // for now: select first item in list (or no item if there are none left)
        UpdateItemDetails();
    }

    public void OnUseButtonClicked()
    {
        selectedItem.UseItem();
        if (selectedItem.GetType() == typeof(Consumable)) {
            UpdateInventory();
        }
    }

    public void SelectItem(IItem selected)
    {
        selectedItem = selected;
        UpdateItemDetails();
    }

    public void UpdateItemDetails()
    {
        if (selectedItem != null)
        {
            ItemTitle.text = selectedItem.title;
            ItemDescription.text = selectedItem.desc;
        }
        else
        {
            ItemTitle.text = "[ INVENTORY ]";
            ItemDescription.text = "Select an item to interact with.";
        }
        UpdateButton();
    }

    public void UpdateButton()
    {
        // Update button depending on item category
        if (selectedItem == null || selectedItem.category == (Category)1) // Misc
        {
            UseItemButton.interactable = false;
            UseItemButtonText.text = "";
        } else {
            if (selectedItem.category == (Category)2) // Consumable
                UseItemButtonText.text = "USE";
            else
                UseItemButtonText.text = "EQUIP"; // Equipment
            UseItemButton.interactable = true;
        }
    }

    // Get item from name
    public IItem GetItemFromName(string name)
    {
        foreach (var kvPair in GameManager.instance.inventory)
        {
            if (kvPair.Key.ToLower() == name.ToLower())
            {
                switch (kvPair.Key.ToLower()) // should probably get a proper DB construct
                {
                    case ("spoon"):
                        return new Equippable("Spoon", "Ladle of Justice", "Perfect for stirring pasta sauce and whacking evildoers. <color=green><b>(2 ATK, +Push)</b></color>", (Category)3, 2, 8f);
                    case ("tiller"):
                        return new Equippable("Tiller", "Dragonslayer Trident", "Legend has it, it really hurts to be hit with one of these. <color=green><b>(4 ATK, ++Push)</b></color>", (Category)3, 4, 12f);
                    case ("dandelion weed"):
                        return new Item("Dandelion Weed", "Dandelion Weed", "Some say that if you gather enough, you can make a wish ... Or something.", (Category)1);
                    case ("horseradish root"):
                        return new Item("Horseradish Root", "Horseradish Root", "Sharp and robust, like the hoof of a raging beast.", (Category)1);
                    case ("milk"):
                        return new HealingItem("Milk", "Bovine Blessing", "Lactose-free. <color=green><b>(100% HEAL)</b></color>", (Category)2, GameManager.instance.player.maxHitpoint);
                    case ("coffee"):
                        return new Item("Coffee", "Energizing Elixir", "Mom says I'm not allowed to drink this.", (Category)1);
                    default: break;

                }
            }
        }
        return null; // Invalid item name...awkward...
    }

}
