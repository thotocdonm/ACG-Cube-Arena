using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected EnemyStatsSO stats;

    [Header("Elements")]
    [SerializeField] protected Transform playerTarget;
    protected Rigidbody rb;
    

    private float currentHealth;
    private float lastAttackTime = 100f;

    private bool canMove = true;

    protected abstract void Attack();

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (stats != null)
        {
            currentHealth = stats.maxHealth;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!canMove || playerTarget == null)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

        ManageRotation();
        ManageMovement();
    }

    private void ManageRotation()
    {
        Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, toRotation, 5f * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }

    }

    private void ManageMovement()
    {
        lastAttackTime += Time.fixedDeltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;

        rb.MovePosition(transform.position + directionToPlayer * stats.moveSpeed * Time.fixedDeltaTime);

        if (distanceToPlayer <= stats.detectionRange)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            if (lastAttackTime >= stats.attackCooldown)
            {
                lastAttackTime = 0f;
                Attack();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{stats.enemyName} died");
        Destroy(gameObject);
    }

    public void StopMovement()
    {
        canMove = false;
    }

    public void ResumeMovement()
    {
        canMove = true;
    }

    // Gizmos giữ nguyên để debug
    protected virtual void OnDrawGizmosSelected()
    {
        if (stats == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);
    }
}
