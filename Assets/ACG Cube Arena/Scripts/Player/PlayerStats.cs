using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] private PlayerStatsSO stats;

    [Header("UI")]
    [SerializeField] private HealthBarUI healthBarUI;
    public Stat MaxHealth { get; private set; }
    public Stat MoveSpeed { get; private set; }
    public Stat AttackDamage { get; private set; }
    public Stat AttackCooldown { get; private set; }
    public Stat CriticalChance { get; private set; }
    public Stat CriticalDamage { get; private set; }
    public Stat DashSpeed { get; private set; }
    public Stat DashDuration { get; private set; }
    public Stat DashCooldown { get; private set; }
    public Stat ProjectileSpeed { get; private set; }
    public GameObject ProjectilePrefab { get; private set; }

    public int CurrentHealth { get; private set; }


    private void Awake()
    {
        MaxHealth = new Stat(stats.maxHealth);
        MoveSpeed = new Stat(stats.moveSpeed);
        AttackDamage = new Stat(stats.attackDamage);
        AttackCooldown = new Stat(stats.attackCooldown);
        CriticalChance = new Stat(stats.criticalChance);
        CriticalDamage = new Stat(stats.criticalDamage);
        DashSpeed = new Stat(stats.dashSpeed);
        DashDuration = new Stat(stats.dashDuration);
        DashCooldown = new Stat(stats.dashCooldown);
        if(stats.projectilePrefab != null)
        {
            ProjectileSpeed = new Stat(stats.projectileSpeed);
            ProjectilePrefab = stats.projectilePrefab;
        }

        CurrentHealth = (int)MaxHealth.GetValue();
        healthBarUI.SetMaxHealth(MaxHealth.GetValue());
        healthBarUI.SetHealth(MaxHealth.GetValue());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    [NaughtyAttributes.Button]
    public void TestTakeDamage()
    {
        TakeDamage(10);
    }

    [NaughtyAttributes.Button]
    public void TestIncreaseAttackDamage()
    {
        IncreaseAttackDamage(10);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + CurrentHealth);
        healthBarUI.SetHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void IncreaseAttackDamage(int amount)
    {
        AttackDamage.AddModifier(new StatModifier(amount, StatModifierType.Flat, "IncreaseAttackDamage"));
    }
    
    private void Die()
    {
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }




}
