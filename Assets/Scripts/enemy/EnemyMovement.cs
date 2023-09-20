using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private SpriteRenderer sr;

    public GameObject Player;
    public GameObject bullet;
    public GameObject bulletParent;

    public NavMeshAgent agent;

    public float detectionRange = 5f;
    public bool followsPlayer = true;
    public float patrolRange = 2f;
    public float shootingRange = 10;

    public float FireRateFireLotsOfBullets = 10;
    public float FireRateFireBullet = 1.5f;

    private float CooldownFireBullet;
    private float CooldownFireLotsOfBullets;

    private Vector2 targetPoint;

    private Vector2 centrePoint;

    [SerializeField]
    private MovementType movementType = MovementType.None;

    [SerializeField]
    private List<GameObject> wayPoints = new List<GameObject>();
    private int CurrentPoint = 0;


    private enum MovementType
    {
        None,
        Random,
        Patrol,
        Boss
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");
        sr = GetComponent<SpriteRenderer>();
        centrePoint = transform.position;
        Bullet.target = Player;
    }

    private void Update()
    {
        EnemyMovementLogic();
    }

    public void EnemyMovementLogic()
    {


        Vector2 movementDirection = agent.velocity.normalized;

        //flip sprite if moving left
        if (movementDirection.x > 0)
        {
            sr.flipX = true;
        }
        else if (movementDirection.x < 0)
        {
            sr.flipX = false;
        }


        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer <= detectionRange && followsPlayer && distanceToPlayer > shootingRange)
        {
            agent.SetDestination(Player.transform.position);
            // Reset timeSincePlayerLeft
            timeSincePlayerLeft = 0f;
        }
        else
        {
            timeSincePlayerLeft += Time.deltaTime;

            switch (movementType)
            {
                case MovementType.None:
                    // nothing
                    break;
                case MovementType.Random:
                    if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                    {
                        Vector2 point;

                        if (RandomPoint(centrePoint, patrolRange, out point))
                        {
                            agent.SetDestination(point);
                            targetPoint = point;
                        }
                    }
                    break;
                case MovementType.Patrol:
                    if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                    {
                        GameObject waypoint = wayPoints[CurrentPoint];
                        Vector2 waypointVector = new Vector2(waypoint.transform.position.x, waypoint.transform.position.y);
                        agent.SetDestination(waypointVector);

                        targetPoint = waypointVector;

                        CurrentPoint++;

                        if (CurrentPoint >= wayPoints.Count)
                            CurrentPoint = 0;
                    }
                    break;
                case MovementType.Boss:

                    if(distanceToPlayer < shootingRange)
                    {
                        agent.SetDestination(gameObject.transform.position);
                    }
                    else if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                    {
                        GameObject waypoint = wayPoints[CurrentPoint];
                        Vector2 waypointVector = waypoint.transform.position;
                        agent.SetDestination(waypointVector);

                        targetPoint = waypointVector;

                        CurrentPoint++;

                        if (CurrentPoint >= wayPoints.Count)
                            CurrentPoint = 0;
                    }

                    //Abilities
                    if (distanceToPlayer <= shootingRange)
                    {
                        if (Time.time >= CooldownFireBullet)
                        {
                            Debug.Log("Starting to fire...");
                            FireBullet();
                            CooldownFireBullet = Time.time + FireRateFireBullet;
                        }

                        if (Time.time >= CooldownFireLotsOfBullets)
                        {
                            Debug.Log("Starting to fire a lot of bullets...");
                            FireLotsOfBullets();
                            CooldownFireLotsOfBullets = Time.time + FireRateFireLotsOfBullets;
                        }
                    }
                    break;
            }
        }

        // Make the enemy face forward
        agent.transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }

    private void FireLotsOfBullets()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject newBullet = Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            Bullet bulletScript = newBullet.GetComponent<Bullet>();

            Vector2 moveDir = (Player.transform.position - transform.position).normalized * i;

            bulletScript.setMovementDirection(moveDir);
            bulletScript.setBulletDamage(0.1f);
        }
    }
    private void FireBullet()
    {
        GameObject newBullet = Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
        Bullet bulletScript = newBullet.GetComponent<Bullet>();

        Vector2 moveDir = (Player.transform.position - transform.position).normalized * 10;

        bulletScript.setMovementDirection(moveDir);
        bulletScript.setBulletDamage(0.5f);
    }

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

    private float timeSincePlayerLeft = 0f;

    #region DebugInfo
    [CustomEditor(typeof(EnemyMovement))]
    public class HandlesAgentScript : Editor
    {
        public void OnSceneGUI()
        {
            EnemyMovement linkedObject = target as EnemyMovement;

            Handles.color = Color.blue;

            EditorGUI.BeginChangeCheck();

            float range = Handles.RadiusHandle(Quaternion.identity, linkedObject.transform.position, linkedObject.detectionRange, false);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Update DetectionRange");
                linkedObject.detectionRange = range;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(targetPoint, Vector3.forward, 0.1f);
        Handles.DrawWireDisc(transform.position, Vector3.forward, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, targetPoint);

        if (movementType == MovementType.Random)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(centrePoint, Vector3.forward, patrolRange);
        }

        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.forward, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
    #endregion
}
