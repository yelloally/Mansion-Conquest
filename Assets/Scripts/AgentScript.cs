using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    public Health healthBar;

    SpriteRenderer sr;
    Rigidbody2D enemyrb;
    Animator enemyAnim;
    Vector2 direction; //movement 

    [SerializeField]
    private List<GameObject> wayPoints = new List<GameObject>();
    private int CurrentPoint = 0;

    [SerializeField]
    private MovementType movementType = MovementType.None;

    [SerializeField]
    private int maxHealth = 3;
    private GameObject Player;

    private float timeSincePlayerLeft = 0f;

    private bool isDead = false;
    private Vector2 startPos;

    private PlayerController playerController;

    public NavMeshAgent agent;
    [SerializeField] private float patrolRange = 2f; //radius of circle
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private bool followsPlayer = true;

    [SerializeField] public Vector2 centrePoint; //centre of the area the agent wants to move around in

    private int healthEnemy;

    //private Vector2 centrePoint;
    private Vector2 targetPoint;

    enum MovementType
    {
        None, //0
        Random, //1
        Patrol, //2
        Boss, //3
    }

    void Start()
    {
        healthEnemy = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0.5f; //stopping distance to a smaller value

        sr = GetComponent<SpriteRenderer>();
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

    public void Damage()
    {
        enemyAnim.SetTrigger("Hit");

        // Reduce enemy's health
        healthEnemy--;

        StartCoroutine(RestoreHealthAfterDelay(7f));

        if (healthEnemy <= 0)
        {
            isDead = true;
            Dying();
        }

        healthBar.SetHealth(healthEnemy);
        //handle player's health reduction
        playerController.PlayerHit(false);
    }

    public void Dying()
    {
        enemyAnim.SetTrigger("Die");

        //disable colliders and set rb to static
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

        Destroy(gameObject, 2);
    }

    //find random point within a specified range
    bool RandomPoint(Vector2 center, float range, out Vector2 result)
    {
        Vector2 randomPoint = center + Random.insideUnitCircle * range; //random point in a circle
        NavMeshHit hit;
        //use navmesh to find a valid pos on navmesh for random point 
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector2.zero;
        return false;
    }

    private float timeSinceLastSpawn = 0f;
    public float spawnInterval = 5f;
    public GameObject enemyPrefab;
    public float interval = 100;
    private float counter = 0;

    //handle enemy movement 
    private void EnemyMovement()
    {
        Vector2 movementDirection = agent.velocity.normalized;

        if (movementDirection.x > 0)
        {
            sr.flipX = true;
        }
        else if (movementDirection.x < 0)
        {
            sr.flipX = false;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer <= detectionRange && followsPlayer)
        {
            //Player in range of detectionRange searching for path to Player
            agent.SetDestination(Player.transform.position);
            timeSincePlayerLeft = 0f;
        }
        else
        {
            //Player is not nearby, start the timer
            timeSincePlayerLeft += Time.deltaTime;

            switch (movementType)
            {
                case MovementType.None:
                    // code block
                    break;
                case MovementType.Random:
                    if (agent.remainingDistance <= agent.stoppingDistance) //done with path
                    {
                        if (!agent.pathPending) //check if a new path is not already calculated
                        {
                            Vector2 point;
                            if (RandomPoint(centrePoint, patrolRange, out point))
                            {
                                agent.SetDestination(point);//ster new destination 

                                targetPoint = point; //store
                            }
                        }
                    }
                    break;
                case MovementType.Patrol:
                    
                    if (agent.remainingDistance <= agent.stoppingDistance) //done with path
                    {
                        if (!agent.pathPending) //check if a new path is not already calculated
                        {
                            GameObject waypoint = wayPoints[CurrentPoint];
                            Vector2 waypointVector = new Vector2(waypoint.transform.position.x, waypoint.transform.position.y);//waypoints gets converted into a vector2
                            agent.SetDestination(waypointVector);

                            targetPoint = waypointVector; //store 

                            CurrentPoint++;

                            if (CurrentPoint >= wayPoints.Count)
                                CurrentPoint = 0;
                        }
                    }
                    break;
                case MovementType.Boss:
                    BossMovement();
                    break;

            }
        }
        agent.transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }

    private void BossMovement()
    {
        //float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        //if (distanceToPlayer <= detectionRange && followsPlayer)
        //{
        //    //player in range 
        //    agent.SetDestination(Player.transform.position);
        //    timeSincePlayerLeft = 0f;

        //    //spawn enemies
        //    timeSinceLastSpawn += Time.deltaTime;
        //    if (timeSinceLastSpawn >= spawnInterval)
        //    {
        //        SpawnEnemies();
        //        timeSinceLastSpawn = 0f;
        //    }
        //}
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            if (!agent.pathPending) //check if a new path is not already calculated
            {
                Vector2 point;
                if (RandomPoint(centrePoint, patrolRange, out point))
                {
                    agent.SetDestination(point);//ster new destination 

                    targetPoint = point; //store
                }
            }
        }
    }

    
    private IEnumerator RestoreHealthAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //restore enemy's health to its maximum value
        healthEnemy = maxHealth;
        healthBar.SetHealth(healthEnemy);
        isDead = false;
    }

    #region DebugInfo
    //custom editor for visualizing detection range in the scene 
    [CustomEditor(typeof(AgentScript))]
    public class HandlesAgentScript : Editor
    {
        public void OnSceneGUI()
        {
            AgentScript linkedObject = target as AgentScript;

            Handles.color = Color.blue;

            EditorGUI.BeginChangeCheck();

            //gets radius handle 
            float range = Handles.RadiusHandle(Quaternion.identity, linkedObject.transform.position, linkedObject.detectionRange, false);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Update DetectionRange");
                linkedObject.detectionRange = range;
            }
        }
    }

    //vizualiaze gizmos in the scene
    private void OnDrawGizmos()
    {
        //draw target
        Handles.color = Color.red;
        Handles.DrawWireDisc(targetPoint, Vector3.forward, 0.1f);
        Handles.DrawWireDisc(transform.position, Vector3.forward, 0.1f);

        //draw a line from enemy to the target
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, targetPoint);

        if (movementType == MovementType.Random)
        {
            //represent the detetection patrol areafor random move
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(centrePoint, Vector3.forward, patrolRange);
        }
        //represent rhe detection range
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.forward, detectionRange);
    }
    #endregion
}
