using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private Collider2D attackColl;


    // Finite state machine
    private enum State { idle, running, jumping, falling, hurt, climb, attack };
    private State state = State.idle;

    //Fighting
    bool isAttacking = false;
    [SerializeField] GameObject attackHitBox;

    //Ladder variables
    [HideInInspector] public bool canClimb;
    [HideInInspector] public bool bottomLadder = false;
    [HideInInspector] public bool topLadder = false;
    private float naturalGravity;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] LayerMask whatIsLadder;
    public float distance;

    // Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 7f;
    [SerializeField] private static float beginningJumpForce = 20f;
    [SerializeField] private float jumpForce = beginningJumpForce;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource footstep;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        attackColl = attackHitBox.GetComponent<Collider2D>();
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        naturalGravity = rb.gravityScale;
        attackHitBox.SetActive(false);

    }

    private void Update()
    {
        
        if (state != State.hurt)
        {
            Attack();
            if (state != State.attack)
            {
                Movement();
                Climb();
            }
        }

        AnimationState();
        anim.SetInteger("state", (int)state); // Sets animation based on enumeratror state



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.cherries += 1;
            PermanentUI.perm.cherryScore.text = PermanentUI.perm.cherries.ToString();
            
        }

        if (collision.tag == "Powerup")
        {
            jumpForce = jumpForce + 10f;
            Destroy(collision.gameObject);
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetPower());

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (state == State.attack)
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                //enemy.Attacked();
                //Jump();
            }
            else
            {

                state = State.hurt;
                HandleHealth(); // Dealts with health, updading UI

                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to my right therefore I should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //Enemy is to my left therefore I should be damaged and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }

        }
    }

    private void HandleHealth()
    {
        PermanentUI.perm.health -= 1;
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        if (PermanentUI.perm.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxisRaw("Horizontal");
        float vDirection = Input.GetAxisRaw("Vertical");
        

        if (canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            state = State.climb;
        }

        // Moving left
        else if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }
        // Moving right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
           
        }
        
        // Jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
        else
        {

        }

    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;


    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire3") && !isAttacking && coll.IsTouchingLayers(ground))
        {
            isAttacking = true;
            state = State.attack;
            
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        state = State.idle;
    }

    void attackHitboxOn()
    {
        attackHitBox.SetActive(true);
    }

    void attackHitboxOff()
    {
        attackHitBox.SetActive(false);
    }
    
    private void AnimationState()
    {
        if (state == State.attack)
        {

        }
        else if (state == State.climb)
        {

        } 
        else if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }

        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 0f)
        {
            // Moving
            state = State.running;
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
            else
            {
                state = State.falling;
            }
            
        }
    }

    private void FootStep()
    {
        footstep.Play();
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(10);
        jumpForce = beginningJumpForce;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void Climb()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        float vDirection = Input.GetAxisRaw("Vertical");
        float hDirection = Input.GetAxisRaw("Horizontal");

        if (hitInfo.collider != null)
        {

            if (Mathf.Abs(vDirection) > 0)
            {
                canClimb = true;
            }
        }
        else
        {
            if (Mathf.Abs(hDirection)  > 0)
            {
                canClimb = false;
            }
        }

        if (canClimb == true && hitInfo.collider != null)
        {
                
            rb.gravityScale = 0;
            state = State.climb;
            if (Mathf.Abs(vDirection) > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, climbSpeed * vDirection);
                anim.speed = 1f;
            } else
            {
                anim.speed = 0f;
            }
                
        }
        else
        {
                rb.gravityScale = naturalGravity;
        }
        
    }
    
}
