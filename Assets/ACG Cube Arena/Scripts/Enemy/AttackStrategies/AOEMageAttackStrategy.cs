using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEMageAttackStrategy : IAttackStrategy
{
    private readonly Enemy owner;
    private readonly Rigidbody rb;
    private readonly Transform playerTarget;
    private readonly EnemyStats stats;
    private readonly LineRenderer chargeIndicator;
    private readonly ParticleSystem chargingVFX;
    private readonly ParticleSystem sparkVFX;

    private readonly float telegraphDuration;
    private readonly float chargeDuration;
    private readonly float recoveryDuration;

    public AOEMageAttackStrategy(Enemy owner, Rigidbody rb, Transform playerTarget, EnemyStats stats, ParticleSystem chargingVFX, ParticleSystem sparkVFX, float chargeDuration, float recoveryDuration)
    {
        this.owner = owner;
        this.rb = rb;
        this.playerTarget = playerTarget;
        this.stats = stats;
        this.chargingVFX = chargingVFX;
        this.sparkVFX = sparkVFX;
        this.chargeDuration = chargeDuration;
        this.recoveryDuration = recoveryDuration;
    }

    public void Execute(Action onComplete)
    {
        owner.StartCoroutine(ChargeAttackSequence(onComplete));
    }

    private IEnumerator ChargeAttackSequence(Action onComplete)
    {
        Animator animator = owner.GetComponent<Animator>();
        // Prepare to attack
        Vector3 targetPosition = playerTarget.position;
        EnableVFX();
        animator.Play("Attack");
        yield return new WaitForSeconds(chargeDuration);

        DisableVFX();
        animator.Play("Idle");
        Debug.Log("Mage Attack Complete");

        yield return new WaitForSeconds(recoveryDuration);


        onComplete?.Invoke();
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
