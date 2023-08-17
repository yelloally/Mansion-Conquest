using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 hitKick = new Vector2(0f, 2f);
    [SerializeField] Transform hurtBox;
    [SerializeField] float attackRadius = 3f;
    [SerializeField] float hitDelay = 2;

    public Animator anim;
    public DynamicJoystick joystick;
    public Button attackButton;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Vector2 direction;
    private bool isHit = false;
    private float currentHitDelay = 0;
    private int health;

    PlayerHealth playerHealth;
    PlayerMovement playerMovement;

    private void Start()
    {
        InitComp();
    }

    //initialize components
    private void InitComp()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();

        //health = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);

        attackButton.onClick.AddListener(Attack);
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (!isHit)
        {
            Vector2 movementDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
            playerMovement.SetMovementDirection(movementDirection);
        }
    }

    private void Update()
    {
        if (!isHit)
        {
            playerMovement.Move();
            HandleHitDelay();
        }
    }

    //timer after taking damage
    private void HandleHitDelay()
    {
        if (currentHitDelay <= 0)
        {
            //check if player is touching an enemy layer
            if (bc.IsTouchingLayers(LayerMask.GetMask("Enemy")))
            {
                PlayerHit(false); //player is hit by enemy
                currentHitDelay = hitDelay;
            }
        }
        else
        {
            currentHitDelay -= Time.deltaTime;
        }
    }

    private bool isAttacking = false;  

    private void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("Attacking");
            //detect enemies in the attack area and damage them
            Collider2D[] enemyToHit = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Enemy"));

            foreach (Collider2D enemy in enemyToHit)
            {
                Debug.Log("EnemyHit");
                enemy.GetComponent<EnemyHealth>()?.Damage_();
            }

            StartCoroutine(ResetAttack());
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);  
        isAttacking = false;
    }


    public void PlayerHit(bool hitByBomb)
    {
        isHit = true;

        rb.velocity = hitKick * new Vector2(-transform.localScale.x, 0f);
        Vector2 sidewayForce = new Vector2(hitKick.x * transform.localScale.x, 0f);

        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        StartCoroutine(ResetHitStatus(hitByBomb));

        if (!hitByBomb)
        {
            playerHealth.TakeDamage(1);
        }

        if (hitByBomb)
        {
           playerHealth.TakeDamage(2);
        }

        Debug.Log("Player's Health: " + health);
    }

    private IEnumerator ResetHitStatus(bool hitByBomb)
    {
        yield return new WaitForSeconds(0.5f);

        if (hitByBomb)
        {
            anim.SetTrigger("BombHit");
        }
        else
        {
            anim.SetTrigger("EnemyHit");
        }

        isHit = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.forward, attackRadius);
    }
}
