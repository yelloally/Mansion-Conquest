using System.Collections;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthRestore = 3.5f;

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
