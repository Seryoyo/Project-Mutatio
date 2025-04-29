using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damageAmount = 1f; 
    public float pushForce = 1f; 
    public float lifeTime = 2f;
    public LayerMask enemyLayer; // Set this to "Actor" layer in Inspector
    
    void Start()
    {
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            Fighter enemy = collision.GetComponent<Fighter>();
            if (enemy != null)
            {
                Damage damage;
                damage.damageAmount = damageAmount;
                damage.pushForce = pushForce;
                damage.origin = transform.position;
                
                enemy.ReceiveDamage(damage);
                Destroy(gameObject);
            }
        }
        
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player") && 
            ((1 << collision.gameObject.layer) & enemyLayer) == 0)
        {
            Destroy(gameObject);
        }
    }
}