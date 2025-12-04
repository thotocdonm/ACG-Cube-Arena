using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] protected HealthBarUI healthBarUI;
    [SerializeField] protected Transform dmgTextAnchor;
    

    [Header("Base Stats")]
    [SerializeField] protected EnemyStatsSO stats;

    [Header("Hit Feedback")]
    [SerializeField] protected float flashDuration;
    [SerializeField] protected Color flashColor = Color.white;
    [SerializeField,ColorUsage(true, true)] protected Color flashColorHDR;
    public static Action<int, Vector3, bool, Vector3> onEnemyHit;
    public static Action<int, bool> onBossHealthChanged;
    protected MeshRenderer[] allRenderers;
    protected Color[] originalColors;
    protected Color[] originalEmissionColors;
    protected Coroutine flashCoroutine;
    protected Coroutine transitionColorCoroutine;
    protected EnemyType enemyType;

    public Stat MaxHealth { get; private set; }
    public Stat MoveSpeed { get; private set; }
    public Stat AttackDamage { get; private set; }
    public Stat AttackCooldown { get; private set; }
    public Stat DetectionRange { get; private set; }
    public Stat AttackRange { get; private set; }
    public Stat ProjectileSpeed { get; private set; }

    public GameObject ProjectilePrefab { get; private set; }


    public int CurrentHealth { get; protected set; }

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

        enemyType = stats.enemyType;
        Debug.Log("Enemy Type: " + enemyType);

        allRenderers = GetComponentsInChildren<MeshRenderer>(true);
        originalColors = new Color[allRenderers.Length];
        originalEmissionColors = new Color[allRenderers.Length];
        for (int i = 0; i < allRenderers.Length; i++)
        {
            originalColors[i] = allRenderers[i].material.color;
            if(allRenderers[i].material.HasProperty("_EmissionColor"))
            {
                originalEmissionColors[i] = allRenderers[i].material.GetColor("_EmissionColor");
            }
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

    public virtual void TakeDamage(int damage, bool isCritical, Vector3 hitPoint)
    {
        CurrentHealth -= damage;
        if(CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        if(healthBarUI != null)
        {
            healthBarUI.SetHealth(CurrentHealth);
        }
   
        onEnemyHit?.Invoke(damage, dmgTextAnchor.position, isCritical, hitPoint);
        if(stats.enemyType == EnemyType.Boss)
        {
            onBossHealthChanged?.Invoke(CurrentHealth, false);
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

    protected IEnumerator FlashCoroutine()
    {
        foreach (MeshRenderer renderer in allRenderers)
        {
            renderer.material.color = flashColor;
            if (renderer.material.HasProperty("_EmissionColor"))
            {
                renderer.material.SetColor("_EmissionColor", flashColorHDR);
            }
        }
        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < allRenderers.Length; i++)
        {
            allRenderers[i].material.color = originalColors[i];
            if (allRenderers[i].material.HasProperty("_EmissionColor"))
            {
                allRenderers[i].material.SetColor("_EmissionColor", originalEmissionColors[i]);
            }
        }
        flashCoroutine = null;
    }

    protected IEnumerator TransitionToColorCoroutine(Color targetColor, Color targetHDRColor, float transitionDuration, float stayDuration)
    {
        foreach (MeshRenderer renderer in allRenderers)
        {
            renderer.material.DOColor(targetColor, transitionDuration);
            if (renderer.material.HasProperty("_EmissionColor"))
            {
                renderer.material.DOColor(targetHDRColor, "_EmissionColor", transitionDuration);
            }
        }
        yield return new WaitForSeconds(stayDuration);

        for (int i = 0; i < allRenderers.Length; i++)
        {
            allRenderers[i].material.DOColor(originalColors[i], transitionDuration);
            if (allRenderers[i].material.HasProperty("_EmissionColor"))
            {
                allRenderers[i].material.DOColor(originalEmissionColors[i], "_EmissionColor", transitionDuration);
            }
        }
    }
    
    public void TransitionToColor(Color targetColor, Color targetHDRColor, float transitionDuration, float stayDuration)
    {
        if(transitionColorCoroutine != null)
        {
            StopCoroutine(transitionColorCoroutine);
        }
        transitionColorCoroutine = StartCoroutine(TransitionToColorCoroutine(targetColor, targetHDRColor, transitionDuration, stayDuration));
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
        MaxHealth.AddModifier(new StatModifier(healthMultiplier * waveNumber, StatModifierType.Percentage, "WaveModifier"));
        AttackDamage.AddModifier(new StatModifier(attackMultiplier * waveNumber, StatModifierType.Percentage, "WaveModifier"));
        SetHealthBarUI();
        CurrentHealth = (int)MaxHealth.GetValue();
    }

    public void Die()
    {
        if(enemyType != EnemyType.WaveMinion)
        {
            WaveManager.instance.OnEnemyDied();
        }
        Destroy(gameObject);
    }

    public EnemyStatsSO GetBaseStats()
    {
        return stats;
    }

    public void SetEnemyType(EnemyType enemyType)
    {
        this.enemyType = enemyType;
    }

    private void SetHealthBarUI()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetMaxHealth(MaxHealth.GetValue());
            healthBarUI.SetHealth(MaxHealth.GetValue());
        }
    }
    
    public void ChangeBaseStats(EnemyStatsSO newStats)
    {
        stats = newStats;
        MaxHealth.SetBaseValue(newStats.maxHealth);
        MoveSpeed.SetBaseValue(newStats.moveSpeed);
        AttackDamage.SetBaseValue(newStats.attackDamage);
        AttackCooldown.SetBaseValue(newStats.attackCooldown);
        DetectionRange.SetBaseValue(newStats.detectionRange);
        AttackRange.SetBaseValue(newStats.attackRange);

        CurrentHealth = (int)MaxHealth.GetValue();
        onBossHealthChanged?.Invoke(CurrentHealth, true);
    }
    
}
