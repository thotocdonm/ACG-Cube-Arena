using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private HealthBarUI healthBarUI;

    [Header("Base Stats")]
    [SerializeField] private EnemyStatsSO stats;

    [Header("Hit Feedback")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Color flashColor = Color.white;
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
        healthBarUI.SetMaxHealth(MaxHealth.GetValue());
        healthBarUI.SetHealth(CurrentHealth);

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
        TakeDamage(10);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        healthBarUI.SetHealth(CurrentHealth);
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

    [NaughtyAttributes.Button]
    public void TestSlow()
    {
        Debug.Log("Applying slow debuff");
        ApplySlowDebuff(10f, 0.5f);
    }

    public void ApplySlowDebuff(float duration, float slowPercentage)
    {
        string slowSource = "TestSlow";
        MoveSpeed.AddModifier(new StatModifier(-slowPercentage, StatModifierType.Percentage, slowSource));
    }

    [NaughtyAttributes.Button]
    public void TestIncreaseAttackDamage()
    {
        IncreaseAttackDamage(10);
    }

    public void IncreaseAttackDamage(int amount)
    {
        AttackDamage.AddModifier(new StatModifier(amount, StatModifierType.Flat, "IncreaseAttackDamage"));
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
