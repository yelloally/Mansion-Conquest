using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float interval = 100;

    private float counter = 0;
    private int enemiesSpawned = 0; //track the number of enemies spawned.

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += 1;

        if (counter >= interval)
        {
            counter = 0;

            if (enemiesSpawned < 3) //check if the number of spawned enemies is less than 3.
            {
                Instantiate(enemyPrefab, transform.position, transform.rotation);
                enemiesSpawned++; //increment the count of spawned enemies.
            }
        }
    }
}
