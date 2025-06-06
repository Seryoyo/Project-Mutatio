using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Mover
{
    private Animator animator;
    private TriggerManager triggerManager;
    public string IdleAnimation;
    public string AttackAnimation;
    public string WalkAnimation;
    public float AttackAnimationDuration;
    public float triggerLength = 1; // 1 meter
    public float chaseLength = 5; // 5 meter chase range
    private bool chasing;
    private bool collidingWithPlayer;

    private Transform playerTransform;
    private Vector3 startingPosition;

    public ContactFilter2D filter;

    // Hitbox
    public BoxCollider2D weaponHitbox; // Weapon hitbox


    protected override void Start()
    {
        base.Start(); // Get usual boxcollider
        animator = GetComponentInChildren<Animator>(); // Get the Animator component from the child PSB
        triggerManager = GetComponentInChildren<TriggerManager>(); // Get the Animator component from the child PSB
        playerTransform = Player.instance.transform;
        startingPosition = transform.position;
    }

    protected void FixedUpdate()
    {
        // Check if player is in range
        if (playerTransform == null)
        {
            playerTransform = Player.instance.transform;
        }
        else
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
            {
                chasing = Vector3.Distance(playerTransform.position, startingPosition) < triggerLength;
                if (chasing)
                {
                    if (!collidingWithPlayer && !animator.GetBool("attacking"))
                    {
                        if (animator != null && WalkAnimation != null) {
                            animator.Play(WalkAnimation);
                        };
                        UpdateMotor((playerTransform.position - transform.position).normalized);
                    }
                    else // colliding w/ player
                    {
                        if (animator != null && AttackAnimation != null)
                            triggerManager.SetAnimTrigger("attacking");
                    }
                }
                else if (animator != null && IdleAnimation != null)
                {
                    animator.Play(IdleAnimation);
                }

                collidingWithPlayer = false;

                /*boxCollider.OverlapCollider(filter, hits);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i] == null)
                        continue;
                    if (hits[i].name == "Player")
                        collidingWithPlayer = true;
                    hits[i] = null;
                }*/

                if (weaponHitbox != null)
                {
                    Collider2D[] hits = Physics2D.OverlapBoxAll((Vector2)weaponHitbox.transform.position, weaponHitbox.size, filter.layerMask);
                    foreach (var hit in hits)
                    {
                        if (hit == null)
                            continue;
                        if (hit.name == "Player")
                            collidingWithPlayer = true;
                    }
                }
            }
            else
            {
                UpdateMotor(startingPosition - transform.position);
                chasing = false;
            }
        }

        FacePlayer();
    }

    protected void FacePlayer()
    {
        float directionToPlayer = (playerTransform.position - transform.position).normalized.x;
        if (directionToPlayer > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    protected override void Death()
    {
        PortalManager.instance.DecrementEnemyCount();
        GameManager.instance.CalculateRandomDrop();
        Destroy(gameObject);
    }

}
