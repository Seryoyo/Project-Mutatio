using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{

    public float collisionDamage = 1.0f;

    public void OnTriggerStay2D(Collider2D coll)
    {
        if (!coll.name.Equals("Player"))
            return;

       Damage dmg =  new Damage()
                        {
                            damageAmount = collisionDamage,
                            origin = Vector3.zero,
                            pushForce = 0
                        };
        Player.instance.ReceiveDamage(dmg);
    }
}
