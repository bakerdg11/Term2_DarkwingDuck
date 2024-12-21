using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    AudioSource audioSource;
    public enum PickupType
    {
        Life,
        Score,
    }


    [SerializeField] PickupType currentCollectible;
    [SerializeField] AudioClip pickupSound;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();

             switch (currentCollectible)
             {
                 case PickupType.Score:
                     GameManager.Instance.score++;
                     break;
                 case PickupType.Life:
                     GameManager.Instance.lives++;
                     break;
             }
            audioSource.PlayOneShot(pickupSound);
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, pickupSound.length);
        }
    }

}