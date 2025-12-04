using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : EnemyStats
{
    [Header("Boss Stats")]
    [SerializeField] private bool isPhase2;

    public static Action onTriggerPhase2;

    public override void TakeDamage(int damage, bool isCritical, Vector3 hitPoint)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
           
        onEnemyHit?.Invoke(damage, dmgTextAnchor.position, isCritical, hitPoint);
        onBossHealthChanged?.Invoke(CurrentHealth, false);
        if (CurrentHealth <= 0)
        {
            if(isPhase2)
            {
                Die();
            }
            else
            {
                isPhase2 = true;
                onTriggerPhase2?.Invoke();
            }
        }

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashCoroutine());
    }
}
