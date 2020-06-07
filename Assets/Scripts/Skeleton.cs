using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{

    float dirX;
    [SerializeField] float moveSpeed = 3f;


    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;



    private Collider2D coll;
    bool facingRight = true;
    Vector3 localScale;




    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        localScale = transform.localScale;
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < leftCap)
        {
            dirX = 1f;
        } else if (transform.position.x > rightCap)
        {
            dirX = -1f;
        }

        if (isAttacking)
        {
            anim.SetBool("isAttacking", true);
            //time = Time.time + attackDelay;
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }

    private void FixedUpdate()
    {
        if(!isAttacking)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        } else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void LateUpdate()
    {
        CheckWhereToFace();
    }

    private void CheckWhereToFace()
    {
        if (dirX > 0)
        {
            facingRight = true;
        }
        else if (dirX < 0)
        {
            facingRight = false;
        }

        if( ((facingRight) && (localScale.x < 0)  ) || (!facingRight) && (localScale.x > 0) )
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;

    }
}
