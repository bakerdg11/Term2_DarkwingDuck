using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    SpriteRenderer sr;
    AudioSource audioSource;

    public Vector2 initialVelocity;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;
    public Projectile projectilePrefab;

    [SerializeField] AudioClip shootSound;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }



    public void Fire()
    {
        if (!sr.flipX)
        {
            Projectile currentProjectile = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
            currentProjectile.initialVelocity = initialVelocity;
        }
        else
        {
            Projectile currentProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            currentProjectile.initialVelocity = new Vector2(-initialVelocity.x, initialVelocity.y);
        }


        if (shootSound)
            audioSource.PlayOneShot(shootSound); //One shot allows more than one audio clip to play at a time





    }


}
