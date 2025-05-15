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
    public RectTransform mutationAmt;
    public RectTransform hpAmt;

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
        UpdateInventory();
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
                IItem entry = GetItemFromName(kv.Key);
                if (entry != null)
                {
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
        if (selectedItem.GetType() == typeof(Consumable)) { // used up item
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
            ItemTitle.text = selectedItem.itemID;
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

    public void UpdateHealthBar()
    {
        if (hpAmt == null)
            return;
        float newLength = Mathf.Max((Player.instance.hitpoint / Player.instance.maxHitpoint) * .98f, 0f);
        hpAmt.localScale = new Vector3(newLength, hpAmt.localScale.y, hpAmt.localScale.z);
    }

    public void UpdateMutationBar()
    {
        if (mutationAmt == null)
            return;
        float newLength = Mathf.Max((Player.instance.mutationPoint / Player.instance.maxMutationPoint) * .98f, 0f);
        mutationAmt.localScale = new Vector3(newLength, mutationAmt.localScale.y, mutationAmt.localScale.z);
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
                    case ("band-aid"):
                        return new HealingItem(
                            name: "Band-Aid",
                            desc: "A standard-issue adhesive bandage. <color=green><b>(15% HEAL)</b></color>",
                            category: (Category)2,
                            healAmt: Player.instance.maxHitpoint * .15f);
                    case ("regeneration tablet"):
                        return new HealingItem(
                            name: "Regeneration Tablet",
                            desc: "An experimental compound that rapidly rebuilds cellular structure. <color=green><b>(FULL HEAL)</b></color>",
                            category: (Category)2,
                            healAmt: Player.instance.maxHitpoint);
                    case ("neural stabilizer"):
                        return new MutationHealingItem(
                            name: "Neural Stabilizer",
                            desc: "Mitigates mutation-induced neural degradation. <color=lightblue><b>(-MUT)</b></color>",
                            category: (Category)2,
                            mutateHealAmt: 2f);
                    case ("psy-delimiter"):
                        return new MutatorItem(
                            name: "Psy-Delimiter",
                            desc: "Amplifies brainwave patterns beyond normal constraints <color=purple><b>(++MUT)</b></color>",
                            category: (Category)2,
                            mutateAmt: 4f);
                    case ("cell fortifier"):
                        return new MutatorItem(
                            name: "Cell Fortifier",
                            desc: "Stabilizes DNA for enhanced resistance. <color=purple><b>(+MUT)</b></color>",
                            category: (Category)2,
                            mutateAmt: 2f);
                    case ("intertia suppressant"):
                        return new MutatorItem(
                            name: "Inertia Supressant",
                            desc: "Optimizes muscle fiber response time. <color=purple><b>(+MUT)</b></color>",
                            category: (Category)2,
                            mutateAmt: 2f);
                    default: break;

                }
            }
        }
        return null; // Invalid item name...awkward...
    }

}
