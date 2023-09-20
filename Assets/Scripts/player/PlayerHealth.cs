using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    public Health healthBar;

    private float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        //set the maximum value for the health bar
        healthBar.SetMaxHealth(maxHealth); 
    }
    //increases the player's health 
    public void AddHealth(float amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHealth);
        healthBar.SetHealth(health);
    }

    //inflicts damage to the player's health
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }

        healthBar.SetHealth(health);
    }
}
