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

    [Header("Boss Stats")]
    [SerializeField] private EnemyStatsSO phase1Stats;
    [SerializeField] private EnemyStatsSO phase2Stats;

    [Header("Boss Visuals Settings")]
    [SerializeField] private GameObject phase1Visual;
    [SerializeField] private GameObject phase2Visual;
    private Transform phase2SpawnPoint;

    [Header("Attack Strategies")]
    private IAttackStrategy normalAttackStrategy;
    private IAttackStrategy crimsonAttackStrategy;
    private IAttackStrategy lightAttackStrategy;
    private IAttackStrategy thunderAttackStrategy;
    private IAttackStrategy windAttackStrategy;
    private IAttackStrategy rootAttackStrategy;

    [Header("Attack Cooldowns")]
    [SerializeField] private float crimsonAttackCooldown;
    [SerializeField] private float lightAttackCooldown;
    [SerializeField] private float thunderAttackCooldown;
    [SerializeField] private float windAttackCooldown;
    [SerializeField] private float rootAttackCooldown;

    [Header("Attack Strategies Color")]
    [SerializeField] private Color normalAttackStrategyColor;
    [SerializeField, ColorUsage(true, true)] private Color normalAttackStrategyHDRColor;

    [SerializeField] private Color crimsonAttackStrategyColor;
    [SerializeField, ColorUsage(true, true)] private Color crimsonAttackStrategyHDRColor;

    [SerializeField] private Color lightAttackStrategyColor;
    [SerializeField, ColorUsage(true, true)] private Color lightAttackStrategyHDRColor;

    [SerializeField] private Color thunderAttackStrategyColor;
    [SerializeField, ColorUsage(true, true)] private Color thunderAttackStrategyHDRColor;

    [SerializeField] private Color windAttackStrategyColor;
    [SerializeField, ColorUsage(true, true)] private Color windAttackStrategyHDRColor;

    [SerializeField] private Color rootAttackStrategyColor;
    [SerializeField, ColorUsage(true, true)] private Color rootAttackStrategyHDRColor;

    private List<IAttackStrategy> attackStrategies = new List<IAttackStrategy>();
    private Dictionary<IAttackStrategy, float> attackStrategyCooldownsDict = new Dictionary<IAttackStrategy, float>();
    private Dictionary<IAttackStrategy, float> attackLastUsedTimeDict = new Dictionary<IAttackStrategy, float>();

    protected override void Awake()
    {
        base.Awake();
        UpdateAttackStrategy();
        BossStats.onTriggerPhase2 += OnTriggerPhase2;
        phase2SpawnPoint = transform;
    }



    protected override void OnDestroy()
    {
        base.OnDestroy();
        BossStats.onTriggerPhase2 -= OnTriggerPhase2;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(AttackPatternCoroutine());
    }

    private void OnTriggerPhase2()
    {
        StartCoroutine(Phase2TransitionCoroutine());
    }

    private IEnumerator Phase2TransitionCoroutine()
    {
        transform.position = phase2SpawnPoint.position;
        GameUIManager.instance.FadeToWhite(2f);
        yield return new WaitForSeconds(2f);
        AudioManager.instance.PlayShellBreakSound();
        UpdateAttackStrategy(true);
        phase1Visual.SetActive(false);
        phase2Visual.SetActive(true);
        enemyStats.ChangeBaseStats(phase2Stats);
        yield return new WaitForSeconds(2f);

    }

    private void UpdateAttackStrategy(bool isPhase2 = false)
    {
        enemyStats = GetEnemyStats();
        normalAttackStrategy = new NormalAttackStrategy(this, rb, animator, playerTarget, enemyStats, chargingVFXPrefab, normalAttackProjectileSpawnPoint);
        crimsonAttackStrategy = new CrimsonAttackStrategy(this, rb, animator, playerTarget, enemyStats);
        lightAttackStrategy = new LightAttackStrategy(this, rb, animator, playerTarget, enemyStats);
        thunderAttackStrategy = new ThunderAttackStrategy(this, rb, animator, playerTarget, enemyStats);
        windAttackStrategy = new WindAttackStrategy(this, rb, animator, playerTarget, enemyStats);
        rootAttackStrategy = new RootAttackStrategy(this, rb, animator, playerTarget, enemyStats);
        List<IAttackStrategy> allStrategies = new List<IAttackStrategy>()
        {
            crimsonAttackStrategy,
            lightAttackStrategy,
            thunderAttackStrategy,
            windAttackStrategy,
            rootAttackStrategy
        };

        attackStrategies.Clear();
        attackStrategyCooldownsDict.Clear();
        attackLastUsedTimeDict.Clear();

        attackStrategies.Add(normalAttackStrategy);

        if (isPhase2)
        {
            attackStrategies.AddRange(allStrategies);
        }
        else
        {
            List<IAttackStrategy> randomStrategies = GetRandomElements(allStrategies, 2);
            attackStrategies.AddRange(randomStrategies);
        }
        
        foreach (var strategy in attackStrategies)
        {
            attackStrategyCooldownsDict.Add(strategy, GetCooldownForStrategy(strategy));
            attackLastUsedTimeDict.Add(strategy, -Mathf.Infinity);
        }
    }

    private List<IAttackStrategy> GetRandomElements(List<IAttackStrategy> source, int count)
    {
        List<IAttackStrategy> temp = new List<IAttackStrategy>(source);
        List<IAttackStrategy> result = new List<IAttackStrategy>();

        for (int i = 0; i < count && temp.Count > 0; i++)
        {
            int index = Random.Range(0, temp.Count);
            result.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return result;
    }

    private float GetCooldownForStrategy(IAttackStrategy strategy)
    {
        if (strategy is CrimsonAttackStrategy) return crimsonAttackCooldown;
        if (strategy is LightAttackStrategy) return lightAttackCooldown;
        if (strategy is ThunderAttackStrategy) return thunderAttackCooldown;
        if (strategy is WindAttackStrategy) return windAttackCooldown;
        if (strategy is RootAttackStrategy) return rootAttackCooldown;
        if (strategy is NormalAttackStrategy) return 0f;

        return 0f;
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
                Debug.Log("Color: " + color);
                Debug.Log("HDR Color: " + hdrColor);
                enemyStats.TransitionToColor(color, hdrColor, 2f, 5f);
            }


            yield return new WaitForSeconds(2f);
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
        else if (attackStrategy == thunderAttackStrategy)
        {
            return thunderAttackStrategyColor;
        }
        else if (attackStrategy == windAttackStrategy)
        {
            return windAttackStrategyColor;
        }
        else if (attackStrategy == rootAttackStrategy)
        {
            return rootAttackStrategyColor;
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
        else if (attackStrategy == thunderAttackStrategy)
        {
            return thunderAttackStrategyHDRColor;
        }
        else if (attackStrategy == windAttackStrategy)
        {
            return windAttackStrategyHDRColor;
        }
        else if (attackStrategy == rootAttackStrategy)
        {
            return rootAttackStrategyHDRColor;
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
