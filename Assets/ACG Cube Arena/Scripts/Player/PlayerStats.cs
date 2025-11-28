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
    public static Action<int> onPlayerHitted;
    public static Action<int> onHealthChanged;

    [Header("UI")]
    [SerializeField] private PlayerHealthBarUI playerHealthBarUI;

    public Stat MaxHealth => statsDictionary[StatType.MaxHealth];
    public Stat MoveSpeed => statsDictionary[StatType.MoveSpeed];
    public Stat AttackDamage => statsDictionary[StatType.AttackDamage];
    public Stat SkillCooldownReduction => statsDictionary[StatType.SkillCooldownReduction];
    public Stat CriticalChance => statsDictionary[StatType.CriticalChance];
    public Stat CriticalDamage => statsDictionary[StatType.CriticalDamage];
    public Stat DashDistance => statsDictionary[StatType.DashDistance];
    public Stat DashDuration => statsDictionary[StatType.DashDuration];
    public Stat DashCooldownReduction => statsDictionary[StatType.DashCooldownReduction];

    public int CurrentHealth { get; private set; }


    private void Awake()
    {

        statsDictionary.Add(StatType.MaxHealth, new Stat(stats.maxHealth));
        statsDictionary.Add(StatType.MoveSpeed, new Stat(stats.moveSpeed));
        statsDictionary.Add(StatType.AttackDamage, new Stat(stats.attackDamage));
        statsDictionary.Add(StatType.SkillCooldownReduction, new Stat(stats.skillCooldownReduction,0,40f));
        statsDictionary.Add(StatType.CriticalChance, new Stat(stats.criticalChance,0,100));
        statsDictionary.Add(StatType.CriticalDamage, new Stat(stats.criticalDamage));
        statsDictionary.Add(StatType.DashDistance, new Stat(stats.dashDistance));
        statsDictionary.Add(StatType.DashDuration, new Stat(stats.dashDuration));
        statsDictionary.Add(StatType.DashCooldownReduction, new Stat(stats.dashCooldownReduction, 0, 40f));

        CurrentHealth = (int)statsDictionary[StatType.MaxHealth].GetValue();
        MaxHealth.OnValueChanged += OnMaxHealthChangedCallback;

        allRenderers = GetComponentsInChildren<MeshRenderer>();
        originalColors = new Color[allRenderers.Length];
        for (int i = 0; i < allRenderers.Length; i++)
        {
            originalColors[i] = allRenderers[i].material.color;
        }

        playerHealthBarUI.Initialize(this);
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
        onPlayerHitted?.Invoke(damage);
        onHealthChanged?.Invoke(CurrentHealth);
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
        GameUIManager.instance.GameOver();
        Destroy(gameObject);
    }

    public void RestoreHealth(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth.GetValue())
        {
            CurrentHealth = (int)MaxHealth.GetValue();
        }
        onHealthChanged?.Invoke(CurrentHealth);
    }
    
    private void OnMaxHealthChangedCallback(float oldMaxHealth, float newMaxHealth)
    {
        int ratio = Mathf.RoundToInt(CurrentHealth / oldMaxHealth);
        Debug.Log("Current Health: " + CurrentHealth + " Old Max Health: " + oldMaxHealth + " New Max Health: " + newMaxHealth + " Ratio: " + ratio);
        CurrentHealth = (int)(ratio * newMaxHealth);
        onHealthChanged?.Invoke(CurrentHealth);
    }



}
