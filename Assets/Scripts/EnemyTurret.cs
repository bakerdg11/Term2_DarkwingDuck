using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    [SerializeField] float projectileFireRate;
    float timeSinceLastFire = 0;

    // This is getting my Player info 
    public Transform player;
    public float detectionDistance = 5f;


    // Start is called before the first frame update
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        base.Start();

        if (projectileFireRate <= 0)
        {
            projectileFireRate = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        if (player != null)
        {
            if (player.position.x < transform.position.x)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }

            // Calculating distance between enemy and player
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Checking detection range
            if (distanceToPlayer <= detectionDistance)
            {

                anim.SetTrigger("Fire");
                timeSinceLastFire = Time.time;
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        if (damage == 9999)
        {
            anim.SetTrigger("RooSquish");
            Destroy(gameObject.transform.gameObject, 1);
            return;
        }

        base.TakeDamage(damage);
    }

}
