using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Item;

public interface IItem
{
    public string itemID { set;  get; } // How it's name in inventory, sprites...
    public string title { set; get; } // What's displayed in the menu
    public string desc { set; get; } // Flavor text
    public Category category { set; get; } // Determines possible interactions
    public string spriteID { set; get; }
    public void UseItem() { }
}

public class Item : IItem
{
    public string itemID { set; get; }
    public string title { set; get; }
    public string desc { set; get; }
    public Category category { set; get; } 
    public string spriteID { set; get; }

    public enum Category
    {
        Misc = 1,
        Consumable = 2,
        Weapon = 3
    }
    
    public Item(string name, string title, string desc, Category category, string spriteID = null)
    {
        this.itemID = name;
        this.title = title;
        this.desc = desc;
        this.category = category;
        if (spriteID != null)
            this.spriteID = spriteID;
        else
            this.spriteID = name;
    }

    public virtual void UseItem() { } // Do nothin by default

    public class Equippable : Item // Weapon
    {
        public int dmg;
        public float pushForce;

        public Equippable(string name, string title, string desc, Category category, int dmg, float pushForce, string spriteID = null)
            : base (name, title, desc, category, spriteID = null) {
            this.dmg = dmg;
            this.pushForce = pushForce;
        }

        // Equip weapon.
        public override void UseItem()
        {
            // Update player, game manager, change stats
            if (this.itemID.ToLower().Equals(GameManager.instance.currentWeapon)) // we unequip
            {
                GameManager.instance.player.UnequipWeapon();
                // TODO: update weapon sprite in inventory menu
            } else
            {
                GameManager.instance.player.EquipWeapon(this);
                // TODO: update weapon sprite in inventory menu
            }
        }
    }

    public class Consumable : Item
    {
        public Consumable(string name, string title, string desc, Category category, string spriteID = null)
            : base(name, title, desc, category, spriteID = null)
        {}

        public override void UseItem()
        {
            ActivateEffect();
            GameManager.instance.TakeItem(this.itemID, 1, true);
            
        }
        public virtual void ActivateEffect() {}
    }

    public class HealingItem : Consumable
    {
        public float healAmt; // How much HP to recover

        public HealingItem(string name, string title, string desc, Category category, float healAmt, string spriteID = null)
            : base(name, title, desc, category, spriteID = null) { 
            this.healAmt = healAmt;
        }

        public override void ActivateEffect()
        {
            GameManager.instance.player.Heal(healAmt);
        }
    }

}
