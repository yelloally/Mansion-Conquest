using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    PlayerController pc;
    EnemyMovement em;

    public Health healthBarEnemy;
    Animator enemyanim;
    
    public int currentHealth;
    private bool isDead = false;

    private int healthEnemy;

    
    public int maxHealth = 3;

    private void Start()
    {
        //healthBarEnemy = GetComponent<Health>();
        pc = GetComponent<PlayerController>();
        em = GetComponent<EnemyMovement>();
        enemyanim = GetComponent<Animator>();

        healthEnemy = maxHealth;
        healthBarEnemy.SetMaxHealth(maxHealth);
        Debug.Log("Health Bar Enemy " + healthBarEnemy); 
        Debug.Log("Player Controller " + pc); 
    }

    private void Update()
    {
        em.EnemyMovementLogic();
    }

    public void Damage_()
    {
        enemyanim.SetTrigger("Hit");
        healthEnemy--;

        //StartCoroutine(RestoreHealthAfterDelay(7f));

        if (healthEnemy <= 0)
        {
            isDead = true;
            Dying();
        }

        healthBarEnemy.SetHealth(healthEnemy);
    }


    public int GetCurrentHealth()
    {
        return healthEnemy;
    }

    private IEnumerator RestoreHealthAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        healthEnemy = maxHealth;
        healthBarEnemy.SetHealth(healthEnemy);
        isDead = false;
    }
    public void Dying()
    {
        enemyanim.SetTrigger("Die");
        Destroy(gameObject, 3);
    }
}
