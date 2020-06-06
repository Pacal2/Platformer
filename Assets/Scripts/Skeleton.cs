using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{

    float dirX;
    [SerializeField] float moveSpeed = 3f;
    private Collider2D coll;
    bool facingRight = false;
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
        if (transform.position.x < -9f)
        {
            dirX = 1f;
        } else if (transform.position.x > 9f)
        {
            dirX = -1f;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
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
