using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : SimpleEnemy
{
    private Collider2D coll;
    //private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        //rb = GetComponent<Rigidbody2D>();
    }

}
