using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3.5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private Vector2 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void Move()
    {
        if (direction != Vector2.zero)
        {
            if (direction.x >= 0f)
            {
                sr.flipX = false;
            }
            else if (direction.x < 0f)
            {
                sr.flipX = true;
            }

            if (anim != null)
            {
                anim.SetBool("Running", true);
            }
        }
        else
        {
            if (anim != null)
            {
                anim.SetBool("Running", false);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed; //set the player's velocity to move 
    }

    public void SetMovementDirection(Vector2 newDirection)
    {
        direction = newDirection; //update the movement direction
    }
}
