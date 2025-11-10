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

    [Header("States")]
    protected StateMachine stateMachine;
    public EnemyChaseState EnemyChaseState { get; private set; }
    public EnemyAttackState EnemyAttackState { get; private set; }

    public IAttackStrategy AttackStrategy { get; protected set; }

    public Rigidbody Rigidbody => rb;
    public EnemyStatsSO Stats => stats;
    public Transform PlayerTarget => playerTarget;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        EnemyChaseState = new EnemyChaseState(this, stateMachine);
        EnemyAttackState = new EnemyAttackState(this, stateMachine);
        stateMachine.Initialize(EnemyChaseState);
        rb.freezeRotation = true;

        if (stats != null)
        {
            currentHealth = stats.maxHealth;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.Initialize(EnemyChaseState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
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
