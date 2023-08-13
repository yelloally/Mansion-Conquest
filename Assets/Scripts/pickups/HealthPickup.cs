using System.Collections;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerhealth = other.GetComponent<PlayerHealth>();
        if (playerhealth != null)
        {
            Debug.Log("Health potion collected!");
            playerhealth.AddHealth(healthRestore);
            Destroy(gameObject);
        }
    }
}
