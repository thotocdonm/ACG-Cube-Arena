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
    [SerializeField] protected Transform playerTarget;
    protected Rigidbody rb;
    private float currentHealth;

    [Header("States")]
    protected StateMachine stateMachine;
    public EnemyChaseState EnemyChaseState { get; private set; }
    public EnemyAttackState EnemyAttackState { get; private set; }

    public IAttackStrategy AttackStrategy { get; protected set; }

    public Rigidbody Rigidbody => rb;
    public Transform PlayerTarget => playerTarget;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        EnemyChaseState = new EnemyChaseState(this, stateMachine);
        EnemyAttackState = new EnemyAttackState(this, stateMachine);
        stateMachine.Initialize(EnemyChaseState);

        EnemyStats = GetComponent<EnemyStats>();


    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        EnemyStats.AttackDamage.OnValueChanged += OnAttackDamageChanged;
        
    }
    

    protected virtual void OnAttackDamageChanged(float newAttackDamage)
    {
        
    }

    protected virtual void OnDestroy()
    {
        EnemyStats.AttackDamage.OnValueChanged -= OnAttackDamageChanged;
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

    public EnemyStats GetEnemyStats()
    {
        return EnemyStats;
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
