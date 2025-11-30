using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMageEnemy : Enemy
{

    [Header("Boss Mage Enemy Settings")]
    [SerializeField] private ParticleSystem chargingVFX;
    [SerializeField] private ParticleSystem sparkVFX;
    [SerializeField] private AoeAttackIndicator aoeAttackIndicator;
    [SerializeField] private GameObject aoeVFXPrefab;
    [SerializeField] private GameObject spawnIndicator;
    [SerializeField] private GameObject[] spawnableEnemies;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float recoveryDuration;
    private EnemyStats enemyStats;

    [Header("Attack Strategies")]
    private IAttackStrategy aoeStrategy;
    private IAttackStrategy summonStrategy;

    protected override void Awake()
    {
        base.Awake();
        UpdateAttackStrategy();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(BossAttackPatternCoroutine());
    }

    private void UpdateAttackStrategy()
    {
        enemyStats = GetEnemyStats();
        aoeStrategy = new BossAoeAttackStrategy(this, rb, animator, playerTarget, enemyStats, chargingVFX, sparkVFX, aoeAttackIndicator, aoeVFXPrefab, chargeDuration, recoveryDuration);
        summonStrategy = new BossSpawnAttackStrategy(this, rb, animator, spawnableEnemies, spawnIndicator, playerTarget, enemyStats, chargingVFX, sparkVFX, aoeAttackIndicator, aoeVFXPrefab, chargeDuration, recoveryDuration);
    }
    
    private IEnumerator BossAttackPatternCoroutine()
    {
        int aoeCount = 0;
        while (true)
        {
            Debug.Log("aoeCount: " + aoeCount);
            if (aoeCount < 2)
            {
                AttackStrategy = aoeStrategy;
                aoeCount++;
            }
            else
            {
                AttackStrategy = summonStrategy;
                aoeCount = 0;
            }

            yield return new WaitUntil(() => IsAttackReady());

            yield return new WaitUntil(() => stateMachine.GetCurrentState() != EnemyAttackState);
            
            yield return new WaitForSeconds(0.5f);
        }
    }
    

}
