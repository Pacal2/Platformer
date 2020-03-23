using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 1f;
    [SerializeField] private float jumpHeight = 1f;

    [SerializeField] private LayerMask ground;

    private Collider2D coll;

    private bool facingLeft = true;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < .1f)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);

            }

        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
        }

    }

    private void Move()
    {
        if (facingLeft)
        {
            // Test to see if we are beyond the leftCap
            if (transform.position.x > leftCap)
            {

                // Make sure sprite is facing right location and if it's not then face the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                // Test to see if I am on the ground, if so jump

                if (coll.IsTouchingLayers(ground))
                {
                    // Jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);

                }
            }
            else
            {
                facingLeft = false;
            }

        }
        else
        {
            if (transform.position.x < rightCap)
            {
                // Make sure sprite is facing right location and if it's not then face the right direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                // Test to see if I am on the ground, if so jump
                if (coll.IsTouchingLayers(ground))
                {
                    // Jump
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }


    
}
