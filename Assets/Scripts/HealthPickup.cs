using System.Collections;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            Debug.Log("Health potion collected!");
            playerController.AddHealth(healthRestore);
            Destroy(gameObject);
        }
    }
}
