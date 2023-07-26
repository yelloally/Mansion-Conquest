using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class BombThrower : MonoBehaviour
{
    Animator bombAnim;
    public GameObject bombPrefab;
    public Transform throwPoint;
    public float throwInterval = 5f;
    public float detectionRange = 5f;

    private GameObject player;
    private bool playerDetected = false;

    void Start()
    {
        bombAnim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        StartCoroutine(ThrowBombs());
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        playerDetected = distanceToPlayer <= detectionRange;
    }

    private IEnumerator ThrowBombs()
    {
        //Endles loops are bad
        while (true)
        {
            if (playerDetected)
            {
                ThrowBomb();
            }

            yield return new WaitForSeconds(throwInterval);
        }
    }

    private void ThrowBomb()
    {
        if (bombPrefab != null && throwPoint != null)
        {
            Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);
                bombAnim.SetTrigger("Pick");
                bombAnim.SetTrigger("Through");
        }
    }
}
