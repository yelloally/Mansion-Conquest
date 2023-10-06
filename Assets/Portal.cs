using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private float TimeToTeleport = 1f;
    [SerializeField] private Transform teleportLocation;
    private float teleportTime;

    private void Start()
    {
    }

    private void Update()
    {
        if(player && isTeleported)
            if(teleportTime > 0f)
            {
                teleportTime -= Time.deltaTime;
            }
            else
            {
                Teleport();
            }
        }
    }

    private void Teleport()
    {
        player.transform.position = teleportLocation.position;
        teleportTime = TimeToTeleport;
        isTeleported = false;
        player = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTeleported = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTeleported = false;
            player = null;
            teleportTime = TimeToTeleport;
        }
    }
}
