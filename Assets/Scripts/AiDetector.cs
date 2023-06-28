using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SVS.AI
{

    public class AiDetector : MonoBehaviour
    {
        [field: SerializeField]
        public bool PlayerDetected { get; private set; }
        public Transform Player { get; private set; }
        public Vector2 DirectionToTarget => target.transform.position - detectorOrigin.position;

        //for BoxCollider
        [Header("Overlap parameters")]
        [SerializeField]
        private Transform detectorOrigin;
        public Vector2 detectorSize = Vector2.one;
        public Vector2 detectorOriginOffset = Vector2.zero;

        public float detectionDelay = 0.3f;

        public LayerMask detectorLayerMask;

        [Header("Gizmos parameters")]
        public Color gizmoIdleColor = Color.green;
        public Color gizmoDetectedColor = Color.red;
        public bool showGizmos = true;

        [Header("Follow the Player")]
        private bool isFollowPlayer = false;
        private float distance;
        private Vector2 startPos;
        public GameObject player;
        Animator animEnemy;
        public float speed = 1f;
        GameObject enemy;
        private bool isStopped = true;


        private GameObject target;

        public GameObject Target
        {
            get => target;
            private set
            {
                target = value;
                PlayerDetected = target != null;
            }
        }

        private void Start()
        {
            animEnemy = GetComponent<Animator>();
            startPos = transform.position;
            StartCoroutine(DetectionCoroutine());
        }

        private void Update()
        {
            OnDrawGizmos();
        }
        IEnumerator DetectionCoroutine()
        {
            yield return new WaitForSeconds(detectionDelay);
            PerformDetection();
            StartCoroutine(DetectionCoroutine());
        }

        public void PerformDetection()
        {
            Collider2D collider =
                Physics2D.OverlapBox(
                    (Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize, 0, detectorLayerMask);
            if (collider != null)
                Target = collider.gameObject;
            else
                target = null;
        }

        private void OnDrawGizmos()
        {


            if (!PlayerDetected && isFollowPlayer)
            {
                isFollowPlayer = false;
                animEnemy.SetBool("IsRunning", true);
            }

            if (PlayerDetected)
            {
                isFollowPlayer = true;
                animEnemy.SetBool("IsRunning", true);
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
            else if (!PlayerDetected && !isFollowPlayer)
            {
                Gizmos.color = gizmoIdleColor;
                Gizmos.DrawCube((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize);
                Debug.Log("come back home");
                transform.position = Vector2.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, startPos) < 0.1f)
                {
                    isStopped = true;
                    animEnemy.SetBool("IsRunning", false);
                }
            }



            //if (showGizmos && detectorOrigin != null)
            //{
            //    Debug.Log("stay in place");
            //    if (distance >= 10 || Vector2.Distance(transform.position, startPos) < 0.1f)
            //    {
            //        Debug.Log("dont  follow");
            //        isFollowPlayer = false;
            //        animEnemy.SetBool("IsRunning", true);
            //        transform.position = Vector2.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            //    }
            //    Gizmos.color = gizmoIdleColor;
            //}
            //if (PlayerDetected)
            //{
            //    isFollowPlayer = true;
            //    animEnemy.SetBool("IsRunning", true);

            //    if (isFollowPlayer)
            //    {
            //        Debug.Log("follow player");
            //        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            //    }
            //    Gizmos.color = gizmoDetectedColor;
            //    Gizmos.DrawCube((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize);
            //}

        }
    }
}

