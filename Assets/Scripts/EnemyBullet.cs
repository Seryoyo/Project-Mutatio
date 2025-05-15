using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 1f;
    public float pushForce = 1f;
    public float lifeTime = 2f;
    public LayerMask playerLayer; // Set to "Player" layer in Inspector

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
        
        // Find player and shoot toward them
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * 10f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if hit player
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                Damage damage = new Damage()
                {
                    damageAmount = this.damageAmount,
                    pushForce = this.pushForce,
                    origin = transform.position
                };
                player.ReceiveDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

    }
}