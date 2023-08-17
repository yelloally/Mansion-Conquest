using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public int damage = 1;
    Rigidbody2D bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        bulletRB = GetComponent<Rigidbody2D>();
        if (target != null)
        {
            Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
            bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
            Destroy(gameObject, 2);
        }
        else
        {
            Debug.LogWarning("target not assigned to bullet");
            Destroy(gameObject); //destroy the bullet if target is not assigned
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet OnTriggerEnter2D: collision with " + collision.gameObject.name); 

        if (collision.gameObject == target)
        {
            Debug.Log("Bullet OnTriggerEnter2D: Collision with target");
            //inflict damage to the player 
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject); //destroy the bullet upon hitting 
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
