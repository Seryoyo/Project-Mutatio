using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : PlayerMover
{
    private Animator animator;
    SpriteRenderer tiller;
    SpriteRenderer spoon;
    Weapon weapon;

    // Animation state names
    private const string IDLE = "PlayerIdle";
    private const string WALK = "PlayerWalk";
    private bool isAttacking = false;
    private float scanRadius = 3f; // npc detection
    public static Player instance;

    public float mutationPoint;
    public float maxMutationPoint;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        hitpoint = maxHitpoint; // start w/ full hp
        mutationPoint = 0;
        DontDestroyOnLoad(gameObject);
    }

    private new void Start()
    {
        base.Start();
        Player.instance = this;
        UpdateHealthBar();
        animator = GetComponentInChildren<Animator>(); // Get the Animator component from the child PSB
        if (animator == null)
            Debug.LogWarning("Animator component not found in children!");
        Inventory.instance.UpdateHealthBar();
        Inventory.instance.UpdateMutationBar();

        /* tiller = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "tiller").GetComponent<SpriteRenderer>();
        spoon = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "spoon").GetComponent<SpriteRenderer>();
        weapon = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "weapon_bone").GetComponent<Weapon>();
        EquipWeapon((Item.Equippable)GameManager.instance.inventoryMenu.GetItemFromName(GameManager.instance.currentWeapon)); // laugh up a storm why don't you */
    }



    private void FixedUpdate()
    {
        if (isAttacking)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");


        // Check if character is moving
        bool isMoving = Mathf.Abs(moveDelta.x) > 0.1f || Mathf.Abs(moveDelta.y) > 0.1f;

        // Update animator w/ movement state
        if (isMoving)
        { // Walk
            animator.Play(WALK);
        }
        else
        { // doin nothin
            animator.Play(IDLE);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, scanRadius);
            NPC nearestNPC = null;

            foreach (Collider2D hit in hits)
            {
                nearestNPC = hit.GetComponent<NPC>();
                if (nearestNPC != null)
                {
                    nearestNPC.ToggleTextbox(); // Only toggle textbox for nearest NPC's message
                    break;
                }
            }
        }

        UpdateMotor(new Vector3(x, y, 0));

    }

    public void EquipWeapon(Item.Equippable newWeapon = null)
    {
        if (newWeapon != null) { 
            GameManager.instance.currentWeapon = newWeapon.itemID;
            weapon.damagePoint = newWeapon.dmg;
            weapon.pushForce = newWeapon.pushForce;
            switch (GameManager.instance.currentWeapon.ToLower())
            {
                case ("tiller"):
                    tiller.enabled = true;
                    spoon.enabled = false;
                    break;
                case ("spoon"):
                    tiller.enabled = false;
                    spoon.enabled = true;
                    break;
                default:
                    break;
            }
        } else // Default, no-weapon value
        {
            UnequipWeapon();
        }
    }

    public void UnequipWeapon()
    {
        GameManager.instance.currentWeapon = "";
        tiller.enabled = false;
        spoon.enabled = false;
        weapon.damagePoint = 1;
        weapon.pushForce = 4f;
    }

    public void ActivateMutation(string mutationName)
    {
        switch (mutationName.ToLower()) {
            case ("psy-delimiter"):
                // Update sprite
                SpriteRenderer red_eyes = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "red_eyes").GetComponent<SpriteRenderer>();
                red_eyes.enabled = true;
                // Update damage/shoot stuff
                // tba!
                return;
            default:
                Debug.Log("ermmmmm... mutation not found...");
                return;
        } 
    }

    public void AddMutationPoints(float mutPt)
    {
        mutationPoint += mutPt;
    }

    public void HealMutation(float mutHealAmt)
    {
        mutationPoint = Mathf.Min(mutationPoint - mutHealAmt, 0);
        UpdateMutationBar();
        Inventory.instance.UpdateMutationBar();
    }

    public override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        Inventory.instance.UpdateHealthBar();
    }

    public override void Heal(float healAmt)
    {
        base.Heal(healAmt);
        Inventory.instance.UpdateHealthBar();
    }

    public bool HasFullHealth()
    {
        return hitpoint >= maxHitpoint;
    }
    
    public bool CanMutate(float addtlPt)
    {
        return (addtlPt + mutationPoint) <= maxMutationPoint;
    }

    public bool HasNoMutationPoints()
    {
        return mutationPoint <= 0;
    }

    protected void UpdateMutationBar() // Visible in inventory UI
    {
        /* if (mutationAmt != null)
        {
            float newLength = Mathf.Max((mutationPoint / maxMutationPoint) * .95f, 0f);
            mutationAmt.localScale = new Vector3(newLength, mutationAmt.localScale.y, mutationAmt.localScale.z);
        
        }*/
    }

    /*
    private void Attack()
    {
        // animator.Play(MALT_ATTACK);
        isAttacking = true;
        StartCoroutine(ResetAttackState(0.6f)); // Make sure it only plays once
                                                // This way seems kind of dumb so i feel like there's a better way
    }

    private IEnumerator ResetAttackState(float duration)
    {
        // Wait for the attack animation to finish
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }*/

    protected override void UpdateHealthBar()
    {
        base.UpdateHealthBar();
        // TODO: Update in inventory UI as well?
    }

    protected override void Death()
    {
        // this is broken
        // change to gameover scene


        // Heal(maxHitpoint);
        // SceneManager.LoadScene("House");
        // transform.position = new Vector3(5.5f, -1.3f, 0);
    }
}