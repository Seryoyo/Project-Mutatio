using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

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

    private int _bulletLevel = 1;
    private int _healthLevel = 1;
    private int _speedLevel = 1;

    public int bulletLevel
    {
        get => _bulletLevel;
        set
        {
            if ((value >= 1 || value <= 3))
                _bulletLevel = value;
        }
    }
    public int healthLevel
    {
        get => _healthLevel;
        set
        {
            if ((value >= 1 || value <= 3))
                _healthLevel = value;
        }
    }
    private int speedLevel
    {
        get => _speedLevel;
        set
        {
            if ((value >= 1 || value <= 3))
                _speedLevel = value;
        }
    }

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
                UpgradeBullet();
                return;
            case ("cell fortifier"):
                UpgradeHealth();
                return;
            case ("inertia suppressant"):
                UpgradeSpeed();
                return;
            default:
                Debug.Log("ermmmmm... mutation not found...");
                return;
        } 
    }

    public void UpgradeBullet()
    {
        if (bulletLevel >= 3 || bulletLevel < 1) return;
        bulletLevel++;
        Debug.Log("Upgraded bullet.");

        var shooter = GetComponent<Shooter>();
        if (bulletLevel == 2)
        {
            // tba... update this to work on a shader instead?
            SpriteRenderer red_eyes = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "red_eyes").GetComponent<SpriteRenderer>();
            red_eyes.enabled = true;
            shooter.bulletPrefab = Resources.Load<GameObject>("Prefabs/BulletButCooler");
            GameManager.instance.ShowText("You feel psychic energy crackling at your fingertips.", 30, UnityEngine.Color.magenta, transform.position, new Vector3(0, 50f, 0), 2f);
        }
        else if (bulletLevel == 3)
        {
            // tba: make ourple with shader
            SpriteRenderer red_eyes = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "red_eyes").GetComponent<SpriteRenderer>();
            red_eyes.enabled = true;
            red_eyes.material.SetFloat("_HsvShift", 120f); // make purble
            red_eyes.material.SetFloat("_HsvSaturation", 2f);
            shooter.bulletPrefab = Resources.Load<GameObject>("Prefabs/BulletButCoolest");
            GameManager.instance.ShowText("You feel the boundaries between your mind and reality blurring.", 30, UnityEngine.Color.magenta, transform.position, new Vector3(0, 50f, 0), 2f);
        }
    }

    public void UpgradeHealth()
    {
        if (healthLevel >= 3) return;
        healthLevel++;
        maxHitpoint += 5; // increase cap but don't heal
        UpdateHealthBar();
        Debug.Log("Upgraded max health.");
        Debug.Log($"hitpoint: {hitpoint}");
        Debug.Log($"maxHitpoint: {maxHitpoint}");
        var arms = GetComponentsInChildren<SpriteRenderer>(true).Where(sr => sr.CompareTag("Arm"));
        if (healthLevel == 2)
        {
            foreach (var arm in arms)
            {
                switch (arm.name)
                {
                    case "larm_s2": case "rarm_s2":
                        arm.enabled = true; break;
                    default:
                        arm.enabled = false; break;
                }
            }
            GameManager.instance.ShowText("You feel your resilience growing.", 30, UnityEngine.Color.magenta, transform.position, new Vector3(0, 50f, 0), 2f);
        }
        else
        {
            foreach (var arm in arms)
            {
                switch (arm.name)
                {
                    case "larm_s3": case "rarm_s3":
                        arm.enabled = true; break;
                    default:
                        arm.enabled = false; break;
                }
            }
            GameManager.instance.ShowText("You feel vitality flooding your being.", 30, UnityEngine.Color.magenta, transform.position, new Vector3(0, 50f, 0), 2f);
        }
    }

    public void UpgradeSpeed()
    {
        if (speedLevel >= 3) return;
        xSpeed += 2f;
        ySpeed += 1.5f;
        speedLevel++;
        Debug.Log("Upgraded speed.");
        Debug.Log($"xSpeed: {xSpeed}");
        Debug.Log($"ySpeed: {ySpeed}");

        var leftLeggy = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "lleg").GetComponent<SpriteRenderer>();
        var rightLeggy = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "rleg").GetComponent<SpriteRenderer>();

        if (speedLevel == 2)
        {
            leftLeggy.material.SetFloat("_GhostBlend", 1f);
            rightLeggy.material.SetFloat("_GhostBlend", 1f);
            GameManager.instance.ShowText("You feel the world slow down around you.", 30, UnityEngine.Color.magenta, transform.position, new Vector3(0, 50f, 0), 2f);
        }
        else
        {
            leftLeggy.material.SetFloat("_HologramBlend", 1f);
            rightLeggy.material.SetFloat("_HologramBlend", 1f);
            GameManager.instance.ShowText("Your reflexes sharpen to impossible heights.", 30, UnityEngine.Color.magenta, transform.position, new Vector3(0, 50f, 0), 2f);
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
    public bool CanMutate(float addtlPt, string mutationType) {
        var maxedLevel = true;
        switch (mutationType.ToLower())
        {
            case ("psy-delimiter"):
                maxedLevel = (bulletLevel >= 3);
                break;
            case ("cell fortifier"):
                maxedLevel = (healthLevel >= 3);
                break;
            case ("inertia suppressant"):
                maxedLevel = (speedLevel >= 3);
                break;
            default: return false;
        }
        return (!maxedLevel) && ((addtlPt + mutationPoint) <= maxMutationPoint);
    }
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