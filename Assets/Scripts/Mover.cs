using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    public float ySpeed = 0.75f;
    public float xSpeed = 1f;


    // Start is called before the first frame update
    protected new virtual void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected void UpdateMotor(Vector3 input)
    {
        Vector2 move;
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);
        FlipSprite(moveDelta.x);
        moveDelta += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        RaycastHit2D[] hits = new RaycastHit2D[1];

        move = new Vector2(0, moveDelta.y) * Time.deltaTime;
        int count = boxCollider.Cast(move.normalized, hits, move.magnitude);
        if (count == 0)
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);

        move = new Vector2(moveDelta.x, 0) * Time.deltaTime;
        count = boxCollider.Cast(move.normalized, hits, move.magnitude);
        if (count == 0)
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
    }



    public void FlipSprite(float xDelta)
    {
        if (xDelta > 0)
            transform.localScale = Vector3.one;
        else if (xDelta < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

}
