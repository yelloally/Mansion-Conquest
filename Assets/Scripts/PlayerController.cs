using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour

{
    public Animator anim;
    public DynamicJoystick joystick;
    public float speed = 5f;

    Rigidbody2D rb;
    SpriteRenderer sr;

    Vector2 direction;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent < SpriteRenderer >();
    }

    private void FixedUpdate()
    {
        direction = new Vector2(joystick.Horizontal, joystick.Vertical);
        rb.velocity = direction * speed;
    }

    void Update()
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
        //if (Camera.main != null)
        //{
        //    Debug.Log("Current Camera: " + Camera.main);
        //}
    }
}