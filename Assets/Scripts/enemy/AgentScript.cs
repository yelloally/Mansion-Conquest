using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    public GameObject Player;
    public NavMeshAgent agent;

    private EnemyHealth enemyHealth;
    private EnemyMovement enemyMovement;
    
//    private float timeSincePlayerLeft = 0f;
    private bool isDead = false;
    private Vector2 startPos;

    private PlayerController playerController;

    [SerializeField] private float patrolRange = 2f; //radius of circle
    [SerializeField] private float detectionRange = 5f;

    private Vector2 centrePoint;
    private Vector2 targetPoint;

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyMovement = GetComponent<EnemyMovement>();
        agent = GetComponent<NavMeshAgent>();

        enemyHealth.healthBarEnemy.SetMaxHealth(enemyHealth.maxHealth);

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        agent.stoppingDistance = 0.5f;

        startPos = transform.position;
        Player = GameObject.Find("Player");

        enemyMovement.agent = agent;
        enemyMovement.Player = Player;
        Debug.Log("Enemy Health " + enemyHealth); 
        Debug.Log("Enemy Movement " + enemyMovement); 
    }

    private void Update()
    {
        enemyMovement.EnemyMovementLogic();
    }

    public void Damage()
    {
        enemyHealth.Damage_();

        if (enemyHealth.GetCurrentHealth() <= 0)
        {
            isDead = true;
            Dying();
        }

       enemyHealth.healthBarEnemy.SetHealth(enemyHealth.GetCurrentHealth());
    }

    private void Dying()
    {
        enemyHealth.Dying();

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

        Rigidbody2D enemyrb = GetComponent<Rigidbody2D>();
        enemyrb.bodyType = RigidbodyType2D.Static;
    }
}
