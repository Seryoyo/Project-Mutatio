using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ItemSlot : MonoBehaviour
{
    public IItem item;
    public int quantity;
    public UnityEngine.UI.Image icon;
    public TextMeshProUGUI quantityText;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        if (icon == null)
            icon = GetComponentInChildren<UnityEngine.UI.Image>(true);
        if (quantityText == null)
            quantityText = GetComponentInChildren<TextMeshProUGUI>(true);
        button.onClick.AddListener(OnSlotClicked);
    }

    // Call after item updated
    public void UpdateInfo()
    {
        if (icon == null)
            icon = GetComponentInChildren<UnityEngine.UI.Image>(true);
        if (quantityText == null)
            quantityText = GetComponentInChildren<TextMeshProUGUI>(true);

        ChangeIcon();
        UpdateQuantity();
    }

    public void ChangeIcon()
    {
        icon.sprite = Resources.Load<Sprite>("Sprites/" + item.itemID);
        enabled = true;
        icon.enabled = true;
        quantityText.enabled = true;
    }

    public void BlankOutSlot()
    {
        enabled = false;
        icon.enabled = false;
        quantityText.enabled = false;
    }

    public void UpdateQuantity()
    {
        quantity = GameManager.instance.inventory[item.itemID];
        if (quantity > 1)
            quantityText.text = quantity.ToString();
        else
            quantityText.text = "";
    }

    public void OnSlotClicked()
    {
        Inventory.instance.SelectItem(item);
    }


}
