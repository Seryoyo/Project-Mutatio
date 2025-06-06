using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // All fighters can receive damage / die
    public float hitpoint;
    public float maxHitpoint;
    public float pushRecoverySpeed = 0.2f;

    // Immunity
    protected float immuneTime = 0.25f;
    protected float lastImmune;
    protected float flickerSpeed = 0.05f;

    protected Vector3 pushDirection;

    public RectTransform hpAmt;

    protected void Start()
    {
        hpAmt = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "hpAmt").GetComponent<RectTransform>();
    }

    public virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, new Vector3(0, 100f, 0), 0.5f);
            UpdateHealthBar();
            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            }
        }
    }

    public virtual void Heal(float dmgAmount)
    {
        hitpoint = Mathf.Min(hitpoint + dmgAmount, maxHitpoint);
        UpdateHealthBar();
    }

    protected virtual void UpdateHealthBar()
    {
        if (hpAmt != null)
        {
            float newLength = Mathf.Max((hitpoint / maxHitpoint) * .95f, 0f);
            hpAmt.localScale = new Vector3(newLength, hpAmt.localScale.y, hpAmt.localScale.z);
        }
    }

    protected IEnumerator IFrameShader(SpriteRenderer sr)
    {
        if (sr.material is Material mat)
        {
            var timer = 0f;
            while (timer < immuneTime)
            {
                mat.SetFloat("_HitEffectBlend", .5f);
                yield return new WaitForSeconds(flickerSpeed);
                mat.SetFloat("_HitEffectBlend", 0f);
                yield return new WaitForSeconds(flickerSpeed);
                timer += flickerSpeed * 2;
            }
            mat.SetFloat("_HitEffectBlend", 0f);
        }
    }

    protected virtual void Death()
    {

    }
}