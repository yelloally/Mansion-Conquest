using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject target;
    public float speed;
    Rigidbody2D bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        bulletRB = GetComponent<Rigidbody2D>();
        if (target != null)
        {
            Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
            bulletRB.velocity = moveDir;
            Destroy(gameObject, 2);
        }
        else
        {
            Debug.LogWarning("target not assigned to bullet");
            Destroy(gameObject); //destroy the bullet if target is not assigned
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
