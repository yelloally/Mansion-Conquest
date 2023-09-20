using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static GameObject target;
    private float _damage = 0.5f;
    private Rigidbody2D bulletRB;
    private Vector2 _moveDirection;

    public void setBulletDamage(float damage)
    {
        _damage = damage;
    }
    public void setMovementDirection(Vector2 movementDirection)
    {
        _moveDirection = movementDirection;
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();

        bulletRB.velocity = new Vector2(_moveDirection.x, _moveDirection.y);
        Destroy(gameObject, 2);

        if (target == null)
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
                playerHealth.TakeDamage(_damage);
            }

            Destroy(gameObject); //destroy the bullet upon hitting 
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
