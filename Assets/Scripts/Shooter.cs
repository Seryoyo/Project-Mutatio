using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{


    public GameObject bulletPrefab;

    Rigidbody2D rigidbody;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVert = Input.GetAxis("ShootVertical");

        if((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVert);
            lastFire = Time.time;
        }
        
    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        
        // PHYSICS STUFF (rigidbody)
        Rigidbody2D bulletRb = bullet.AddComponent<Rigidbody2D>();
        bulletRb.gravityScale = 0;
        bulletRb.velocity = new Vector2(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed, 
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed
        );
        
        // Add bullet controller with damage values
        BulletController bc = bullet.AddComponent<BulletController>();
        bc.damageAmount = 1f; 
        bc.pushForce = 1f; 
        
        CircleCollider2D collider = bullet.AddComponent<CircleCollider2D>();
        collider.isTrigger = true; 
        collider.radius = 0.1f; 
    }

}
