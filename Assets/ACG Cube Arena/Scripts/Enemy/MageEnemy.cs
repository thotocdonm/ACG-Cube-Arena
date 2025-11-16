using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemy : Enemy
{

    [Header("Mage Attack Settings")]
    [SerializeField] private ParticleSystem chargingVFX;
    [SerializeField] private ParticleSystem sparkVFX;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float recoveryDuration;
    private EnemyStats enemyStats;

    protected override void Awake()
    {
        base.Awake();
        UpdateAttackStrategy();
    }
    
    private void UpdateAttackStrategy()
    {
        enemyStats = GetEnemyStats();
        AttackStrategy = new AOEMageAttackStrategy(this, rb, playerTarget, enemyStats, chargingVFX, sparkVFX, chargeDuration, recoveryDuration);
    }
}
