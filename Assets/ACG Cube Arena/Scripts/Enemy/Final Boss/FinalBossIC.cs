using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossIC : Enemy
{
    [Header("Final Boss IC Settings")]
    [SerializeField] private GameObject spawnIndicator;
    [SerializeField] private float recoveryDuration;
    [SerializeField] private GameObject chargingVFXPrefab;
    [SerializeField] private Transform normalAttackProjectileSpawnPoint;
    private EnemyStats enemyStats;

    [Header("Attack Strategies")]
    private IAttackStrategy normalAttackStrategy;
    private IAttackStrategy crimsonAttackStrategy;
    private IAttackStrategy lightAttackStrategy;

    [Header("Attack Cooldowns")]
    [SerializeField] private float crimsonAttackCooldown;
    [SerializeField] private float lightAttackCooldown;

    [Header("Attack Strategies Color")]
    [SerializeField] private Color normalAttackStrategyColor;
    [SerializeField, ColorUsage(true, true)] private Color normalAttackStrategyHDRColor;

    [SerializeField] private Color crimsonAttackStrategyColor;
    [SerializeField, ColorUsage(true, true)] private Color crimsonAttackStrategyHDRColor;

    [SerializeField] private Color lightAttackStrategyColor;
    [SerializeField, ColorUsage(true, true)] private Color lightAttackStrategyHDRColor;

    private List<IAttackStrategy> attackStrategies = new List<IAttackStrategy>();
    private List<float> attackStrategyCooldowns = new List<float>();
    private Dictionary<IAttackStrategy, float> attackStrategyCooldownsDict = new Dictionary<IAttackStrategy, float>();
    private Dictionary<IAttackStrategy, float> attackLastUsedTimeDict = new Dictionary<IAttackStrategy, float>();

    protected override void Awake()
    {
        base.Awake();
        UpdateAttackStrategy();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(AttackPatternCoroutine());
    }

    private void UpdateAttackStrategy()
    {
        enemyStats = GetEnemyStats();
        normalAttackStrategy = new NormalAttackStrategy(this, rb, animator, playerTarget, enemyStats, chargingVFXPrefab, normalAttackProjectileSpawnPoint);
        crimsonAttackStrategy = new CrimsonAttackStrategy(this, rb, animator, playerTarget, enemyStats);
        lightAttackStrategy = new LightAttackStrategy(this, rb, animator, playerTarget, enemyStats);


        attackStrategies.Add(crimsonAttackStrategy);
        attackStrategies.Add(lightAttackStrategy);

        attackStrategyCooldowns.Clear();
        attackStrategyCooldowns.Add(crimsonAttackCooldown);
        attackStrategyCooldowns.Add(lightAttackCooldown);

        for (int i = 0; i < attackStrategies.Count; i++)
        {
            attackStrategyCooldownsDict.Add(attackStrategies[i], attackStrategyCooldowns[i]);
            attackLastUsedTimeDict[attackStrategies[i]] = -Mathf.Infinity;
        }

    }


    private IEnumerator AttackPatternCoroutine()
    {
        while (true)
        {
            IAttackStrategy nextAttackStrategy = ChooseNextAttackStrategy();
            Debug.Log("Next Attack Strategy: " + nextAttackStrategy.GetType().Name);
            AttackStrategy = nextAttackStrategy;
            if (nextAttackStrategy != normalAttackStrategy)
            {
                animator.Play("Rotation");
                Color color = GetColorForAttackStrategy(nextAttackStrategy);
                Color hdrColor = GetHDRColorForAttackStrategy(nextAttackStrategy);
                enemyStats.TransitionToColor(color, hdrColor, 2f, 3f);
            }


            yield return new WaitForSeconds(3f);
            animator.Play("Idle");

            yield return new WaitUntil(() => IsAttackReady());

            yield return new WaitUntil(() => stateMachine.GetCurrentState() != EnemyAttackState);

            if (nextAttackStrategy != normalAttackStrategy)
            {
                attackLastUsedTimeDict[nextAttackStrategy] = Time.time;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IAttackStrategy ChooseNextAttackStrategy()
    {
        List<IAttackStrategy> availableStrategies = new List<IAttackStrategy>();
        foreach (var strategy in attackStrategies)
        {
            attackLastUsedTimeDict.TryGetValue(strategy, out float lastUsedTime);

            if (Time.time >= lastUsedTime + attackStrategyCooldownsDict[strategy])
            {
                availableStrategies.Add(strategy);
            }
        }

        if (availableStrategies.Count > 0)
        {
            return availableStrategies[Random.Range(0, availableStrategies.Count)];
        }
        else
        {
            return normalAttackStrategy;
        }
    }

    private Color GetColorForAttackStrategy(IAttackStrategy attackStrategy)
    {
        if (attackStrategy == crimsonAttackStrategy)
        {
            return crimsonAttackStrategyColor;
        }
        else if (attackStrategy == lightAttackStrategy)
        {
            return lightAttackStrategyColor;
        }
        else
        {
            return normalAttackStrategyColor;
        }

    }

    private Color GetHDRColorForAttackStrategy(IAttackStrategy attackStrategy)
    {
        if (attackStrategy == crimsonAttackStrategy)
        {
            return crimsonAttackStrategyHDRColor;
        }
        else if (attackStrategy == lightAttackStrategy)
        {
            return lightAttackStrategyHDRColor;
        }
        else
        {
            return normalAttackStrategyHDRColor;
        }
    }

        #if UNITY_EDITOR
    private void OnGUI()
    {
        GUI.Label(new Rect(500, 10, 200, 20), $"State: {stateMachine.CurrentState.GetType().Name}");
    }
    #endif
}
