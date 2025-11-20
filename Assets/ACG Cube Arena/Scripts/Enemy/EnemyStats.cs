using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] protected Transform dmgTextAnchor;
    

    [Header("Base Stats")]
    [SerializeField] private EnemyStatsSO stats;

    [Header("Hit Feedback")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Color flashColor = Color.white;
    public static Action<int, Vector3, bool, Vector3> onEnemyHit;
    public static Action<int> onBossHealthChanged;
    private MeshRenderer[] allRenderers;
    private Color[] originalColors;
    private Coroutine flashCoroutine;

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

        CurrentHealth = (int)MaxHealth.GetValue();
        SetHealthBarUI();

        allRenderers = GetComponentsInChildren<MeshRenderer>();
        originalColors = new Color[allRenderers.Length];
        for (int i = 0; i < allRenderers.Length; i++)
        {
            originalColors[i] = allRenderers[i].material.color;
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

    [NaughtyAttributes.Button]
    public void TestDmg()
    {
        TakeDamage(10, false, transform.position);
    }

    public void TakeDamage(int damage, bool isCritical, Vector3 hitPoint)
    {
        CurrentHealth -= damage;
        if(healthBarUI != null)
        {
            healthBarUI.SetHealth(CurrentHealth);
        }
   
        onEnemyHit?.Invoke(damage, dmgTextAnchor.position, isCritical, hitPoint);
        if(stats.enemyType == EnemyType.Boss)
        {
            onBossHealthChanged?.Invoke(CurrentHealth);
        }
        if (CurrentHealth <= 0)
        {
            Die();
        }

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        foreach(MeshRenderer renderer in allRenderers)
        {
            renderer.material.color = flashColor;
        }
        yield return new WaitForSeconds(flashDuration);
        
        for (int i = 0; i < allRenderers.Length; i++)
        {
            allRenderers[i].material.color = originalColors[i];
        }
        flashCoroutine = null;
    }


    public void ApplySlowDebuff(float duration, float slowPercentage)
    {
        string slowSource = "TestSlow";
        MoveSpeed.AddModifier(new StatModifier(-slowPercentage, StatModifierType.Percentage, slowSource));
    }


    public void IncreaseAttackDamage(int amount)
    {
        AttackDamage.AddModifier(new StatModifier(amount, StatModifierType.Flat, "IncreaseAttackDamage"));
    }

    public void ApplyWaveModifier(int waveNumber, float healthMultiplier, float attackMultiplier)
    {
        Debug.Log("Applying Wave Modifier: Health Multiplier: " + healthMultiplier * waveNumber + " Attack Multiplier: " + attackMultiplier * waveNumber);
        MaxHealth.AddModifier(new StatModifier(healthMultiplier * waveNumber, StatModifierType.Percentage, "WaveModifier"));
        AttackDamage.AddModifier(new StatModifier(attackMultiplier * waveNumber, StatModifierType.Percentage, "WaveModifier"));
        SetHealthBarUI();
        CurrentHealth = (int)MaxHealth.GetValue();
    }

    public void Die()
    {
        Debug.Log("Enemy Defeated");
        WaveManager.instance.OnEnemyDied();
        Destroy(gameObject);
    }

    public EnemyStatsSO GetBaseStats()
    {
        return stats;
    }

    private void SetHealthBarUI()
    {
        if(healthBarUI != null)
        {
            healthBarUI.SetMaxHealth(MaxHealth.GetValue());
            healthBarUI.SetHealth(MaxHealth.GetValue());
        }
    }
    
}
