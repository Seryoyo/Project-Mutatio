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
<<<<<<< HEAD
            lastImmune = Time.time;
            foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            {
                StartCoroutine(IFrameShader(sr));
=======
            lastImmune = Time.time;
            foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            {
                StartCoroutine(IFrameShader(sr));
>>>>>>> d6b260c (fixed portal)
            }
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