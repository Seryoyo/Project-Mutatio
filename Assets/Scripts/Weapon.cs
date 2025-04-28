using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon : Collidable
{
    public Fighter owner;
    public int damagePoint = 1;
    public float pushForce = 4.0f;
    // private SpriteRenderer spriteRenderer;

    // Swing
    private float cooldown = 0.5f;
    private float lastSwing;


    protected override void Start()
    {
        base.Start();
        // spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll) {
        if (coll.tag == "Fighter") {

            // Check if the collider is on the same side (enemy/player), e.g. prevent enemies damaging each other or player damaging itself
            if (owner.gameObject.layer == coll.gameObject.layer)
                return;

            Damage dmg = new Damage()
            {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
        
    }

    private void Swing() {
    }

}
