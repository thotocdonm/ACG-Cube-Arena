using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyStats))]
public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    private EnemyStats EnemyStats;

    [Header("Elements")]
    protected Transform playerTarget;
    [SerializeField] protected BoxCollider meleeHitbox;
    [SerializeField] protected LayerMask obstacleLayer;
    protected Rigidbody rb;
    protected Animator animator;
    private float attackCooldownTimer = 0f;



    [Header("States")]
    protected StateMachine stateMachine;
    public EnemyChaseState EnemyChaseState { get; private set; }
    public EnemyAttackState EnemyAttackState { get; private set; }

    public IAttackStrategy AttackStrategy { get; protected set; }

    public Rigidbody Rigidbody => rb;
    public Transform PlayerTarget => playerTarget;
    public StateMachine StateMachine => stateMachine;
    public LayerMask ObstacleLayer => obstacleLayer;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
        animator = GetComponent<Animator>();
        stateMachine = new StateMachine();
        EnemyChaseState = new EnemyChaseState(this, stateMachine);
        EnemyAttackState = new EnemyAttackState(this, stateMachine);
        stateMachine.Initialize(EnemyChaseState);

        EnemyStats = GetComponent<EnemyStats>();


    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        
    }
    

    protected virtual void OnDestroy()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if(attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        stateMachine.Update();
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public EnemyStats GetEnemyStats()
    {
        return EnemyStats;
    }

    public void EnableHitbox()
    {
        meleeHitbox.enabled = true;
    }
    public void DisableHitbox()
    {
        meleeHitbox.enabled = false;
    }

    public bool IsAttackReady()
    {
        return attackCooldownTimer <= 0;
    }

    public void SetAttackCooldown(float cooldown)
    {
        attackCooldownTimer = cooldown;
    }
    
    // Gizmos giữ nguyên để debug
    protected virtual void OnDrawGizmosSelected()
    {
        if (EnemyStats == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, EnemyStats.DetectionRange.GetValue());
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyStats.AttackRange.GetValue());
    }
}
