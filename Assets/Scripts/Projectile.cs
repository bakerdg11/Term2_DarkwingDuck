using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime;
    public Vector2 initialVelocity;


    // Start is called before the first frame update
    void Start()
    {
        if (lifeTime <= 0)
        {
            lifeTime = 2.0f;
        }

        GetComponent<Rigidbody2D>().velocity = initialVelocity;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile") && CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(10);
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
