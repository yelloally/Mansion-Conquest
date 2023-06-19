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
    Vector2 direction;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (direction.x < 0f)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
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
}