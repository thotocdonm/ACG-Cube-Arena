using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChargeAttackStrategy : IAttackStrategy
{
    private readonly Enemy owner;
    private readonly Rigidbody rb;
    private readonly Transform playerTarget;
    private readonly EnemyStats stats;
    private readonly LineRenderer chargeIndicator;

    private readonly float telegraphDuration;
    private readonly float chargeSpeed;
    private readonly float chargeDuration;
    private readonly float recoveryDuration;

    public ChargeAttackStrategy(Enemy owner, Rigidbody rb, Transform playerTarget, EnemyStats stats, LineRenderer chargeIndicator, float telegraphDuration, float chargeSpeed, float chargeDuration, float recoveryDuration)
    {
        this.owner = owner;
        this.rb = rb;
        this.playerTarget = playerTarget;
        this.stats = stats;
        this.chargeIndicator = chargeIndicator;
        this.telegraphDuration = telegraphDuration;
        this.chargeSpeed = chargeSpeed;
        this.chargeDuration = chargeDuration;
        this.recoveryDuration = recoveryDuration;
    }

    public void Execute(Action onComplete)
    {
        Debug.Log("Current damage: " + stats.AttackDamage.GetValue());
        owner.StartCoroutine(ChargeAttackSequence(onComplete));
    }

    private IEnumerator ChargeAttackSequence(Action onComplete)
    {
        // Prepare to attack
        Vector3 directionToPlayer = (playerTarget.position - owner.transform.position).normalized;
        if (chargeIndicator != null)
        {
            chargeIndicator.enabled = true;
            chargeIndicator.SetPosition(0, owner.transform.position);
            chargeIndicator.SetPosition(1, owner.transform.position + directionToPlayer * stats.AttackRange.GetValue() * 1.5f);
        }
        yield return new WaitForSeconds(telegraphDuration);

        //Charging toward player
        chargeIndicator.enabled = false;
        Vector3 targetPosition = owner.transform.position + directionToPlayer * chargeSpeed * 1.5f;
        rb.DOMove(targetPosition, chargeDuration).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            Debug.Log("Charged");
        });

        yield return new WaitForSeconds(recoveryDuration);

        onComplete?.Invoke();
    }
}
