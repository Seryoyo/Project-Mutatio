using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFighter : Fighter
{

    protected float PlayerImmuneTime = 1.0f;

    public override void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > PlayerImmuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, 
                                        transform.position, new Vector3(0, 100f, 0), 0.5f);
            UpdateHealthBar();
            
            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            }
        }
    }
}