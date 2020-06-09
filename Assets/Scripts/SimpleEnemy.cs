using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioSource death;

    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        death = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "AttackHitBox")
        {
            anim.SetTrigger("Death");
            death.Play();
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<Collider2D>().enabled = false;
        }
    }


    private void Death()
    {
        Destroy(this.gameObject);
    }
}
