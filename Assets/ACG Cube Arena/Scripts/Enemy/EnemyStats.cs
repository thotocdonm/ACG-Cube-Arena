using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    [Header("Base Stats")]
    [SerializeField] private EnemyStatsSO stats;

    public Stat MaxHealth { get; private set; }
    public Stat MoveSpeed { get; private set; }
    public Stat AttackDamage { get; private set; }
    public Stat AttackCooldown { get; private set; }
    public Stat DetectionRange { get; private set; }
    public Stat AttackRange { get; private set; }
    public Stat ProjectileSpeed { get; private set; }

    public GameObject ProjectilePrefab { get; private set; }


    public int CurrentHealth { get; private set; }

    void Awake()
    {
        MaxHealth = new Stat((int)stats.maxHealth);
        MoveSpeed = new Stat((int)stats.moveSpeed);
        AttackDamage = new Stat((int)stats.attackDamage);
        AttackCooldown = new Stat((int)stats.attackCooldown);
        DetectionRange = new Stat((int)stats.detectionRange);
        AttackRange = new Stat((int)stats.attackRange);
        ProjectileSpeed = new Stat((int)stats.projectileSpeed);
        ProjectilePrefab = stats.projectilePrefab;

        CurrentHealth = MaxHealth.GetValue();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy Defeated");
        Destroy(gameObject);
    }

    public EnemyStatsSO GetBaseStats()
    {
        return stats;
    }
    
}
