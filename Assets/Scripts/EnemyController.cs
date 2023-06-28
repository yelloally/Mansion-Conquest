using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D enemyrb;
    Animator enemyAnim;

    public GameObject[] point = new GameObject[13]; //waypoints for enemy
    private int action, rand = 0;
    public float speed = 1f;
    public int health = 3;
    private GameObject Player;

    //private float distance;
    // private int action, rand = 0;
    private bool isDead = false;
    private Vector2 startPos;
    private bool isReturn = false;

    void Start()
    {
        enemyrb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        //startPos = transform.position;
        Player = GameObject.Find("Player");
        startPos = transform.position;
    }

    void Update()
    {
        EnemyMovement();
    }

    private void FixedUpdate()
    {
        
    }

    public void Damage()
    {
        Debug.Log("DAMAGE");

        do
        {
            Debug.Log("NOT DEAD");
            enemyAnim.SetTrigger("Hit");

            health--; //reduce health
            Debug.Log("health after hit " + health);

            if (health <= 0)
            {
                isDead = true;
                break;
            }
            return;
        }
        while (isDead == true);
        Dying();
    }

    public void Dying()
    {
        Debug.Log("enemy dead");
        enemyAnim.SetTrigger("Die");

        CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = false;
        }
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        enemyrb.bodyType = RigidbodyType2D.Static;

        StartCoroutine(EnemyDestroy()); //destroy the enemy
    }

    IEnumerator EnemyDestroy()
    {
        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }

    private void EnemyMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);

        if (distanceToPlayer <= 5f)
        {
            //find the player
            action = 1;
            Vector2 direction = (Player.transform.position - transform.position).normalized;
            enemyrb.AddForce(enemyrb.velocity = direction * speed);
        }
        else
        {
            //lose the player
            action = 0;
            if (!isReturn)
            {
                if (Vector2.Distance(transform.position, startPos) > 3f)
                {
                    //go to the start pos
                    enemyrb.AddForce(transform.position = Vector2.MoveTowards(transform.position, startPos, speed * Time.deltaTime));
                }
                else
                {
                    //player has moved away , return to start pos
                    //isReturn = true;
                    Debug.Log("returned " + isReturn);
                    EnemyAtThePos();

                    action = 0;
                }
            }

            else
            {
                Debug.Log(isReturn + "???");
                //reache starpos
                isReturn = false;
            }
            enemyrb.AddForce(enemyrb.velocity = Vector2.zero);
        }

        
    }

    private void EnemyAtThePos()
    {
        if (point[rand].transform.parent != null)
            for (int i = 0; i < point.Length; i++)
            {
                point[i].transform.parent = null;
            }
        if (transform.position != point[rand].transform.position)
            enemyrb.AddForce(transform.position = Vector2.MoveTowards(transform.position, point[rand].transform.position, speed * Time.deltaTime));
        else
            rand = Random.Range(0, 13);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (action == 0)
            rand = Random.Range(0, 12);
    }
}
