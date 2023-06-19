using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator anim;
    public DynamicJoystick joystick;
    float speed = 0f;

    Rigidbody2D rb;
    Vector2 direction;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;
        rb.AddForce(speed * Time.deltaTime * direction, ForceMode2D.Impulse);
    }

    void Update()
    {
      if ( direction != new Vector2(0, 0))
        {
           
        }
    }
}
