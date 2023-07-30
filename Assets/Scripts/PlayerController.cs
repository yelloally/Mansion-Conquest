using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour

{
    [SerializeField] int maxHealth = 10;
    [SerializeField] float speed = 3.5f;
    [SerializeField] Vector2 hitKick = new Vector2(0f, 2f);
    [SerializeField] Transform hurtBox;
    [SerializeField] float attackRadius = 3f;


    public Animator anim;
    public DynamicJoystick joystick;
    public Button attackButon;

    public Health healthBar;

    public Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D bc;

    Vector2 direction; //movement 

    bool isHit = false;

    private int health;


    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        attackButon.onClick.AddListener(Attack);
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent < SpriteRenderer >();
        bc = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (!isHit)
        {
            direction = new Vector2(joystick.Horizontal, joystick.Vertical);
            rb.velocity = direction * speed; //move based on the joystick input
        }
    }


    [SerializeField]
    private float hitDelay = 2;
    private float currentHitDelay = 0;
    void Update()
    {
        if (!isHit)
        {
            Move();

            if(currentHitDelay <= 0)
            {
                if (bc.IsTouchingLayers(LayerMask.GetMask("Enemy")))
                {
                    PlayerHit(false); //when player collides with enemy
                    currentHitDelay = hitDelay;
                }
            }
            else
            {
                currentHitDelay -= Time.deltaTime;
            }
        }
    }

    public void Attack()
    {
        anim.SetTrigger("Attacking");

        Collider2D[] enemyToHit = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Enemy"));

        foreach(Collider2D enemy in enemyToHit)
        {
            Debug.Log("EnemyHit");
            enemy.GetComponent<AgentScript>()?.Damage();

        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage; //reduce player's health 

        //check if the player is still alive
        if (health <= 0)
        {
            //player is dead
            //disable the player's GameObject
            gameObject.SetActive(false);
        }

        healthBar.SetHealth(health);
    }

    public void PlayerHit(bool hitByBomb)
    {
        isHit = true;

        rb.velocity = hitKick * new Vector2(-transform.localScale.x, 0f); //apply kick force
        Vector2 sidewayForce = new Vector2(hitKick.x * transform.localScale.x, 0f);

        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        StartCoroutine(ResetHitStatus(hitByBomb)); //reset the hit status after delay

        // Reduce player's health by 1 when hit by an enemy
        if (!hitByBomb)
        {
            TakeDamage(1);
        }

        if (hitByBomb)
        {
            TakeDamage(2);
        }
        Debug.Log("Player's Health: " + health);
    }

    IEnumerator ResetHitStatus(bool hitByBomb)
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("EnemyHit");

        if(hitByBomb)
        {
            anim.SetTrigger("BombHit");
        }
        else
        {
            anim.SetTrigger("EnemyHit");
        }

        isHit = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }



    private void Move()
    {
        if (direction != Vector2.zero)
        {
            if (direction.x >= 0f)
            {
                sr.flipX = false; //flip the sprite if mowing right
            }
            else if (direction.x < 0f)
            {
                sr.flipX = true; //flip the sprite if moving left
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

    private void OnDrawGizmosSelected()
    {
      //  Gizmos.DrawLine(hurtBox.position, attackRadius); //draw a sphere to represent attack radius
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.forward, attackRadius);
    }
}