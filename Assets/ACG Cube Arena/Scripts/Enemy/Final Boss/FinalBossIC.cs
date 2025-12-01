using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossIC : Enemy
{
    [Header("Boss Mage Enemy Settings")]
    [SerializeField] private GameObject spawnIndicator;
    [SerializeField] private float recoveryDuration;
    private EnemyStats enemyStats;

    [Header("Attack Strategies")]
    private IAttackStrategy crimsonAttackStrategy;
    private IAttackStrategy summonStrategy;

    protected override void Awake()
    {
        base.Awake();
        UpdateAttackStrategy();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void UpdateAttackStrategy()
    {
        enemyStats = GetEnemyStats();
        crimsonAttackStrategy = new CrimsonAttackStrategy(this, rb, animator, playerTarget, enemyStats);
        AttackStrategy = crimsonAttackStrategy;
    }
    

    
}
