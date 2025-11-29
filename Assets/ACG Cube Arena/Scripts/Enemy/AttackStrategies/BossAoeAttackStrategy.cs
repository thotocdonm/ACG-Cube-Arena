using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BossAoeAttackStrategy : IAttackStrategy
{
    private readonly Enemy owner;
    private readonly Rigidbody rb;
    private readonly Animator animator;
    private readonly Transform playerTarget;
    private readonly EnemyStats stats;
    private readonly ParticleSystem chargingVFX;
    private readonly ParticleSystem sparkVFX;
    private readonly AoeAttackIndicator aoeAttackIndicator;
    private readonly GameObject aoeVFXPrefab;
    private readonly float chargeDuration;
    private readonly float recoveryDuration;

    private readonly int numberOfAttack = 5;
    private readonly float attackInterval = 0.3f;

    public BossAoeAttackStrategy(Enemy owner, Rigidbody rb, Animator animator, Transform playerTarget, EnemyStats stats, ParticleSystem chargingVFX, ParticleSystem sparkVFX, AoeAttackIndicator aoeAttackIndicator, GameObject aoeVFXPrefab, float chargeDuration, float recoveryDuration)
    {
        this.owner = owner;
        this.rb = rb;
        this.animator = animator;
        this.playerTarget = playerTarget;
        this.stats = stats;
        this.chargingVFX = chargingVFX;
        this.sparkVFX = sparkVFX;
        this.aoeAttackIndicator = aoeAttackIndicator;
        this.aoeVFXPrefab = aoeVFXPrefab;
        this.chargeDuration = chargeDuration;
        this.recoveryDuration = recoveryDuration;
    }

    public void Execute(Action onComplete)
    {
        owner.StartCoroutine(BossAoeAttackSequence(onComplete));
    }

    private IEnumerator BossAoeAttackSequence(Action onComplete)
    {
        EnableVFX();
        animator.Play("AoeAttacking");

        for (int i = 0; i < numberOfAttack; i++)
        {
            owner.StartCoroutine(SingleAoeRoutine());
            yield return new WaitForSeconds(attackInterval);
        }

        yield return new WaitForSeconds(chargeDuration - (numberOfAttack - 1) * attackInterval);

        DisableVFX();
        animator.Play("Idle");

        yield return new WaitForSeconds(Mathf.Max(0, recoveryDuration));

        onComplete?.Invoke();
    }
    
    private IEnumerator SingleAoeRoutine()
    {
        // Prepare
        Vector3 playerPosition = playerTarget.position;

        Vector2 randomOffset2D = UnityEngine.Random.insideUnitCircle * 10;
        Vector3 randomOffset = new Vector3(randomOffset2D.x, 0, randomOffset2D.y);

        Vector3 targetPosition = playerPosition + randomOffset;
        targetPosition.y = 0.7f;

        // Show Indicator
        AoeAttackIndicator aoeAttackIndicatorInstance = VFXPoolManager.instance.enemyAoeIndicatorPool.Get().GetComponent<AoeAttackIndicator>();
        aoeAttackIndicatorInstance.transform.position = targetPosition;
        aoeAttackIndicatorInstance.transform.rotation = Quaternion.identity;
        aoeAttackIndicator.SetRadius(10f);
        aoeAttackIndicatorInstance.StartExpanding(chargeDuration);
        DOVirtual.DelayedCall(chargeDuration + 0.3f, () => VFXPoolManager.instance.enemyAoeIndicatorPool.Release(aoeAttackIndicatorInstance.gameObject));
        float radius = aoeAttackIndicatorInstance.GetComponentInChildren<MeshRenderer>().bounds.extents.magnitude * 0.6f;
        yield return new WaitForSeconds(chargeDuration);

        //Spawn VFX
        GameObject aoeVFXInstance = VFXPoolManager.instance.enemyAoeVFXPool.Get();
        aoeVFXInstance.transform.position = targetPosition;
        aoeVFXInstance.transform.localScale = new Vector3(1, 1, 1);
        DOVirtual.DelayedCall(2f, () => VFXPoolManager.instance.enemyAoeVFXPool.Release(aoeVFXInstance));

        yield return new WaitForSeconds(0.3f);

        AudioManager.instance.PlayMageAttackSound();
        Collider[] colliders = Physics.OverlapSphere(targetPosition, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                collider.gameObject.GetComponent<PlayerStats>().TakeDamage((int)stats.AttackDamage.GetValue());
            }
        }
        
    }

    private void DisableVFX()
    {
        sparkVFX.Stop();
        sparkVFX.gameObject.SetActive(false);
        chargingVFX.Stop();
        chargingVFX.gameObject.SetActive(false);
    }
    
    private void EnableVFX()
    {
        sparkVFX.gameObject.SetActive(true);
        sparkVFX.Play();
        chargingVFX.gameObject.SetActive(true);
        chargingVFX.Play();
    }
}
