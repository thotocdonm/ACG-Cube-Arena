using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MageEnemy : Enemy
{

    [Header("Mage Attack Settings")]
    [SerializeField] private ParticleSystem chargingVFX;
    [SerializeField] private ParticleSystem sparkVFX;
    [SerializeField] private AoeAttackIndicator aoeAttackIndicator;
    [SerializeField] private GameObject aoeVFXPrefab;
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
        AttackStrategy = new AOEMageAttackStrategy(this, rb, animator, playerTarget, enemyStats, chargingVFX, sparkVFX, aoeAttackIndicator, aoeVFXPrefab, chargeDuration, recoveryDuration);
    }

    public void PlayMageChargeSound()
    {
        AudioManager.instance.PlayMageChargeSound();
    }
    
}
