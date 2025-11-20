using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] private PlayerStatsSO stats;
    private Dictionary<StatType, Stat> statsDictionary = new Dictionary<StatType, Stat>();

    [Header("Damage Settings")]
    [SerializeField] private float iFrameDuration = 0.5f;
    private float lastHitTime;

    [Header("Hit Feedback")]
    [SerializeField] private float flashInterval = 0.1f;
    private MeshRenderer[] allRenderers;
    private Color[] originalColors;
    private Coroutine flashCoroutine;
    public static Action onPlayerHitted;

    [Header("UI")]
    [SerializeField] private HealthBarUI healthBarUI;

    public Stat MaxHealth => statsDictionary[StatType.MaxHealth];
    public Stat MoveSpeed => statsDictionary[StatType.MoveSpeed];
    public Stat AttackDamage => statsDictionary[StatType.AttackDamage];
    public Stat AttackCooldown => statsDictionary[StatType.AttackCooldown];
    public Stat SkillCooldown => statsDictionary[StatType.SkillCooldown];
    public Stat CriticalChance => statsDictionary[StatType.CriticalChance];
    public Stat CriticalDamage => statsDictionary[StatType.CriticalDamage];
    public Stat DashSpeed => statsDictionary[StatType.DashSpeed];
    public Stat DashDuration => statsDictionary[StatType.DashDuration];
    public Stat DashCooldown => statsDictionary[StatType.DashCooldown];

    public int CurrentHealth { get; private set; }


    private void Awake()
    {
        statsDictionary.Add(StatType.MaxHealth, new Stat(stats.maxHealth));
        statsDictionary.Add(StatType.MoveSpeed, new Stat(stats.moveSpeed));
        statsDictionary.Add(StatType.AttackDamage, new Stat(stats.attackDamage));
        statsDictionary.Add(StatType.AttackCooldown, new Stat(stats.attackCooldown));
        statsDictionary.Add(StatType.SkillCooldown, new Stat(stats.skillCooldown));
        statsDictionary.Add(StatType.CriticalChance, new Stat(stats.criticalChance));
        statsDictionary.Add(StatType.CriticalDamage, new Stat(stats.criticalDamage));
        statsDictionary.Add(StatType.DashSpeed, new Stat(stats.dashSpeed));
        statsDictionary.Add(StatType.DashDuration, new Stat(stats.dashDuration));
        statsDictionary.Add(StatType.DashCooldown, new Stat(stats.dashCooldown));

        CurrentHealth = (int)statsDictionary[StatType.MaxHealth].GetValue();
        healthBarUI.SetMaxHealth(statsDictionary[StatType.MaxHealth].GetValue());
        healthBarUI.SetHealth(statsDictionary[StatType.MaxHealth].GetValue());
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
    public void TestTakeDamage()
    {
        TakeDamage(10);
    }

    [NaughtyAttributes.Button]
    public void TestIncreaseAttackDamage()
    {
        IncreaseAttackDamage(10);
    }

    public Stat GetStat(StatType statType)
    {
        if (statsDictionary.TryGetValue(statType, out Stat stat))
        {
            return stat;
        }
        Debug.LogWarning("Stat not found: " + statType);
        return null;
    }

    public void TakeDamage(int damage, bool isBypassIframe = false)
    {

        if (!isBypassIframe)
        {
            if (Time.time < lastHitTime + iFrameDuration)
            {
                Debug.Log("Player is in iFrame. Taking no damage.");
                return;
            }

            lastHitTime = Time.time;
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(IFrameFlashCoroutine());
        }

        CurrentHealth -= damage;
        onPlayerHitted?.Invoke();
        Debug.Log("Player took " + damage + " damage. Current health: " + CurrentHealth);
        healthBarUI.SetHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator IFrameFlashCoroutine()
    {
        Debug.LogWarning("Starting iFrame flash coroutine");
        bool isWhite = false;
        while (Time.time < lastHitTime + iFrameDuration)
        {
            isWhite = !isWhite;
            Color targetColor = isWhite ? Color.white : originalColors[0];

            for (int i = 0; i < allRenderers.Length; i++)
            {
                allRenderers[i].material.color = targetColor;
            }
            yield return new WaitForSeconds(flashInterval);
        }
        
        for (int i = 0; i < allRenderers.Length; i++)
        {
            allRenderers[i].material.color = originalColors[i];
        }
        flashCoroutine = null;
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
