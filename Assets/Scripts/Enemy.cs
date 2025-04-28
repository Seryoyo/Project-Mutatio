using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Mover
{
    private Animator animator;
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
    private Collider2D[] hits = new Collider2D[10];

    public string droppedItem = null;


    protected override void Start()
    {
        base.Start(); // Get usual boxcollider
        animator = GetComponentInChildren<Animator>(); // Get the Animator component from the child PSB
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
    }

    protected void FixedUpdate()
    {
        // Check if player is in range
        if (playerTransform == null)
        {
            playerTransform = GameManager.instance.player.transform;
        }
        else
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
            {
                chasing = Vector3.Distance(playerTransform.position, startingPosition) < triggerLength;
                if (chasing)
                {
                    if (!collidingWithPlayer)
                    {
                        if (animator != null && WalkAnimation != null) { animator.Play(WalkAnimation); }
                        ;
                        UpdateMotor((playerTransform.position - transform.position).normalized);
                    }
                    else // colliding w/ player
                    {
                        if (animator != null && AttackAnimation != null)
                        {
                            animator.Play(AttackAnimation);
                            StartCoroutine(ResetAttackState(AttackAnimationDuration)); // Make sure it only plays once
                        }

                    }
                }
                else if (animator != null && IdleAnimation != null)
                {
                    animator.Play(IdleAnimation);
                }

                collidingWithPlayer = false;
                boxCollider.OverlapCollider(filter, hits);
                foreach (var hit in hits)
                {
                    if (hits == null)
                        continue;
                    if (hits.name == "Player")
                        collidingWithPlayer = true;
                    hits = null;
                }

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
        if (droppedItem != null && droppedItem.Length >= 1)
        {
            GameManager.instance.GrantItem(droppedItem, 1);
        }
        // And show text for it
        Destroy(gameObject);
    }

    private IEnumerator ResetAttackState(float duration)
    {
        // Wait for the attack animation to finish
        yield return new WaitForSeconds(duration);
    }

}
