using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;
    public float moveSpeed = 5.0f;
    private Animator animator;

    // Animation state names
    private const string MALT_WALK = "MaltWalk";
    private const string MALT_IDLE = "MaltIdle";
    private const string MALT_ATTACK = "MaltAttack";
    private bool isAttacking = false;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>(); // Get the Animator component from the child PSB

        if (animator == null)
            Debug.LogWarning("Animator component not found in children!");
    }

    private void FixedUpdate()
    {
        if (isAttacking)
            return; // No moving while attacking... for now.
                    // Later: Maybe add attack cancellation and only deal damage at a certain point in the animation

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Reset moveDelta
        moveDelta = new Vector3(x, y, 0) * moveSpeed;

        // Check if character is moving
        bool isMoving = Mathf.Abs(moveDelta.x) > 0.1f || Mathf.Abs(moveDelta.y) > 0.1f;

        // Update animator w/ movement state
        if (animator != null)
        {
            if (!isMoving && Input.GetKeyDown(KeyCode.E)) // Attack!!!
            {
                animator.Play(MALT_ATTACK);
                isAttacking = true;
                StartCoroutine(ResetAttackState()); // Make sure it only plays once
            }
            else if (isMoving) { // Walk
                animator.Play(MALT_WALK);
            }
            else { // doin nothin
                animator.Play(MALT_IDLE);
            }
        }

        // Swap sprite direction
        FlipSprite(moveDelta.x);

        // Make sure we can move in this direction by casting a box there first.
        // If the box returns null, we're free to move.
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
    }

    public void FlipSprite(float xDelta)
    {
        if (xDelta  > 0)
            transform.localScale = Vector3.one;
        else if (xDelta < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private IEnumerator ResetAttackState()
    {
        // Wait for the attack animation to finish
        yield return new WaitForSeconds(0.4f); // It's 0.45 really but who cares man
        isAttacking = false;
    }
}