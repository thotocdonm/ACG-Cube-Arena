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
    private IAttackStrategy summonStrategy;

    [Header("Attack Cooldowns")]
    [SerializeField] private float crimsonAttackCooldown;

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


        attackStrategies.Add(crimsonAttackStrategy);

        attackStrategyCooldowns.Clear();
        attackStrategyCooldowns.Add(crimsonAttackCooldown);

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

            yield return new WaitUntil(() => IsAttackReady());
            stateMachine.ChangeState(EnemyAttackState);
            yield return new WaitUntil(() => stateMachine.GetCurrentState() != EnemyAttackState);

            if (nextAttackStrategy != normalAttackStrategy)
            {
                attackLastUsedTimeDict[nextAttackStrategy] = Time.time;
            }
            
            yield return new WaitForSeconds(2f);
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

    
}
