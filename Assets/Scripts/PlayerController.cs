using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    //Component
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    AudioSource audioSource;

    //Inspector balance variables
    [SerializeField] float speed = 5.0f;
    [SerializeField] int jumpForce = 7;

    //Groundcheck 
    [SerializeField] bool isGrounded;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask isGroundLayer;
    [SerializeField] float groundCheckRadius = 0.02f;

    //Audio Clips
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip stompSound;



    //Lives and Score
 /*   [SerializeField] int maxLives = 5;
    private int _lives;
    public int lives
    {
        get => _lives;
        set
        {
            _lives = value;
        }
    }    

    private int _score = 0;
    public int score
    {
        get => score;
        set
        {
            _score = value;
        }
    }
 */



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (speed <= 0)
        {
            speed = 5.0f;
        }

        if (jumpForce <= 0)
        {
            jumpForce = 7;
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.02f;
        }

        if (GroundCheck == null)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            GroundCheck = obj.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        // Character Movement Variables
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, isGroundLayer);
        if (isGrounded)
        {
            rb.gravityScale = 1;
            anim.ResetTrigger("JumpAtk");
        }

        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);


        // Calling Character Movement
        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);


        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpSound);
        }

        if (clipInfo[0].clip.name == "Fire")
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetTrigger("Fire");
            }
        }

        // Determine if player is moving upwards or downwards
        bool isMovingUpwards = rb.velocity.y > 0;
        bool isMovingDownwards = rb.velocity.y < 0;

        // Set animation parameters based on vertical movement
        anim.SetBool("JumpUp", isMovingUpwards);
        anim.SetBool("JumpDown", isMovingDownwards);

        anim.SetFloat("Input", Mathf.Abs(xInput));
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("Crouch", yInput);




        anim.SetFloat("Input", Mathf.Abs(xInput));
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("Crouch", yInput);

        //Sprite Flipping
        if (xInput != 0) sr.flipX = (xInput < 0);

    }



    //Trigger functions are called most times - but still generally require one physics body
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            GameManager.Instance.lives--;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Squish"))
        {
            collision.transform.parent.gameObject.GetComponent<Enemy>().TakeDamage(9999);
            Destroy(collision.gameObject);

            //do our bouncy stuff here
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            audioSource.PlayOneShot(stompSound);
        }

        if (collision.CompareTag("Collectible"))
        {
            GameManager.Instance.score++;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("GameComplete"))
        {
            SceneManager.LoadScene("GameOver");
        }


    }


    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }


    //Collisions functions are only called when one of two objects that collide is a dynamic rigidbody
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            GameManager.Instance.lives--;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }












}
