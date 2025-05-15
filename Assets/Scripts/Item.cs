using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Item;

public interface IItem
{
    public string itemID { set;  get; } // How it's named in inventory, sprites
    public string desc { set; get; } // Flavor text
    public Category category { set; get; } // Determines possible interactions
    public string spriteID { set; get; }
    public void UseItem() { }
    public bool CanUseItem();
}

public class Item : IItem
{
    public string itemID { set; get; }
    public string desc { set; get; }
    public Category category { set; get; } 
    public string spriteID { set; get; }

    public enum Category
    {
        Misc = 1,
        Consumable = 2,
        Weapon = 3
    }
    
    public Item(string name, string desc, Category category, string spriteID = null)
    {
        this.itemID = name;
        this.desc = desc;
        this.category = category;
        if (spriteID != null)
            this.spriteID = spriteID;
        else
            this.spriteID = name;
    }

    public virtual void UseItem() { } // Do nothin by default
    public virtual bool CanUseItem() => false;

    // this game doesn't use this 
    public class Equippable : Item // Weapon
    {
        public int dmg;
        public float pushForce;

        public Equippable(string name, string desc, Category category, int dmg, float pushForce, string spriteID = null)
            : base (name, desc, category, spriteID = null) {
            this.dmg = dmg;
            this.pushForce = pushForce;
        }

        // Equip weapon
        public override void UseItem()
        {
            /*
            // Update player, game manager, change stats
            if (this.itemID.ToLower().Equals(GameManager.instance.currentWeapon)) // we unequip
            {
                Player.instance.UnequipWeapon();
                // TODO: update weapon sprite in inventory menu
            } else
            {
                Player.instance.EquipWeapon(this);
                // TODO: update weapon sprite in inventory menu
            }*/
        }
    }

    public class Consumable : Item
    {
        public Consumable(string name, string desc, Category category, string spriteID = null)
            : base(name, desc, category, spriteID = null) {}

        public override void UseItem()
        {
            if (ActivateEffect())
                GameManager.instance.TakeItem(this.itemID, 1, true);
            Inventory.instance.UpdateInventory();
            
        }
        public virtual bool ActivateEffect() { return false; }
    }

    public class HealingItem : Consumable
    {
        public float healAmt; // How much HP to recover

        public HealingItem(string name, string desc, Category category, float healAmt, string spriteID = null)
            : base(name, desc, category, spriteID = null) { 
            this.healAmt = healAmt;
        }

        public override bool ActivateEffect()
        {
            if (!CanUseItem()) 
                return false;
            Player.instance.Heal(healAmt);
            return true;
        }

        public override bool CanUseItem() => (!Player.instance.HasFullHealth());
    }

    public class MutatorItem : Consumable
    {
        public float mutateAmt; // How much to add to mutation bar

        public MutatorItem(string name, string desc, Category category, float mutateAmt, string spriteID = null)
            : base(name, desc, category, spriteID = null)
        {
            this.mutateAmt = mutateAmt;
        }

        public override bool ActivateEffect()
        {
            if (!CanUseItem()) // make sure mutation bar has room
                return false;
            Player.instance.ActivateMutation(itemID);
            Player.instance.AddMutationPoints(mutateAmt);
            Inventory.instance.UpdateMutationBar();
            return true;
        }

        public override bool CanUseItem() => (Player.instance.CanMutate(mutateAmt, itemID));
    }

    // Reduce mutation side effect... meter... thing
    public class MutationHealingItem : Consumable
    {
        public float mutateHealAmt; // How much of mutation meter to reduce

        public MutationHealingItem(string name, string desc, Category category, float mutateHealAmt, string spriteID = null)
            : base(name, desc, category, spriteID = null)
        {
            this.mutateHealAmt = mutateHealAmt;
        }

        public override bool ActivateEffect()
        {
            if (!CanUseItem()) // nothing to heal 
                return false;
            Player.instance.HealMutation(mutateHealAmt);
            return true;
        }

        public override bool CanUseItem() => !Player.instance.HasNoMutationPoints();
    }

}
