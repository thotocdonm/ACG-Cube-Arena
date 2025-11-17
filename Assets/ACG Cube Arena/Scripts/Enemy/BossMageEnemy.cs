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

    private void UpdateAttackStrategy()
    {
        enemyStats = GetEnemyStats();
        AttackStrategy = new BossAoeAttackStrategy(this, rb, animator, playerTarget, enemyStats, chargingVFX, sparkVFX, aoeAttackIndicator, aoeVFXPrefab, chargeDuration, recoveryDuration);
    }
    

}
