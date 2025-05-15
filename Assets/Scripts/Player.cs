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

    // Animation state names
    private const string IDLE = "PlayerIdle";
    private const string WALK = "PlayerWalk";
    private bool isAttacking = false;
    private float scanRadius = 3f; // npc detection
    public static Player instance;

    // Mutations and stat upgrades
    public float mutationPoint;
    public float maxMutationPoint;
    
    // public int bulletLevel;
    // public int speedLevel;
    // what other upgrades... idk
    // oh yeah :-) max hp! increase...


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
        instance = this;
        UpdateHealthBar();
        animator = GetComponentInChildren<Animator>();
        Inventory.instance.UpdateHealthBar();
        Inventory.instance.UpdateMutationBar();
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
            animator.Play(WALK);
        else
            animator.Play(IDLE);

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

    public void ActivateMutation(string mutationName)
    {
        // tba... change to work with mutation levels
        switch (mutationName.ToLower()) {
            case ("psy-delimiter"):
                // tba... update this to work on a shader instead
                SpriteRenderer red_eyes = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "red_eyes").GetComponent<SpriteRenderer>();
                red_eyes.enabled = true;
                GetComponent<Shooter>().bulletPrefab = Resources.Load<GameObject>("Prefabs/BulletButCooler");
                return;
            default:
                Debug.Log("ermmmmm... mutation not found...");
                return;
        } 
    }

    public void AddMutationPoints(float mutPt) {
        mutationPoint += mutPt;
    }

    public void HealMutation(float mutHealAmt)
    {
        mutationPoint = Mathf.Max(mutationPoint - mutHealAmt, 0);
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

    public bool HasFullHealth() => (hitpoint >= maxHitpoint);
    public bool CanMutate(float addtlPt) => ((addtlPt + mutationPoint) <= maxMutationPoint);
    public bool HasNoMutationPoints() => (mutationPoint <= 0);
    protected override void UpdateHealthBar() => base.UpdateHealthBar();

    protected override void Death()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(Inventory.instance.gameObject);
        Destroy(Player.instance.gameObject);

        SceneManager.LoadScene("Death");
    }
    
}