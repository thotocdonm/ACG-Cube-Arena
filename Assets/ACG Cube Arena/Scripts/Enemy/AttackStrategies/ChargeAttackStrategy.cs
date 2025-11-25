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
        owner.StartCoroutine(ChargeAttackSequence(onComplete));
    }

    private IEnumerator ChargeAttackSequence(Action onComplete)
    {
        Animator animator = owner.GetComponent<Animator>();
        // Prepare to attack
        Vector3 directionToPlayer = (playerTarget.position - owner.transform.position).normalized;
        if (chargeIndicator != null)
        {
            chargeIndicator.enabled = true;
            chargeIndicator.SetPosition(0, owner.transform.position);
            chargeIndicator.SetPosition(1, owner.transform.position + directionToPlayer * stats.AttackRange.GetValue() * 1.5f);
        }
        animator.Play("Charging");
        yield return new WaitForSeconds(telegraphDuration);

        //Charging toward player
        chargeIndicator.enabled = false;
        Vector3 targetPosition = GetSafeTargetPosition(directionToPlayer, stats.AttackRange.GetValue());
        animator.Play("Attack");
        rb.DOMove(targetPosition, chargeDuration).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            animator.Play("Idle");
        });

        yield return new WaitForSeconds(recoveryDuration);


        onComplete?.Invoke();
    }
    

    private Vector3 GetSafeTargetPosition(Vector3 direction, float distance)
    {
        Vector3 startPosition = owner.transform.position;
        Vector3 targetPosition = startPosition + direction * distance;

        Debug.DrawRay(startPosition, direction * distance, Color.red, 2f);
        

        CapsuleCollider collider = owner.GetComponent<CapsuleCollider>();
        if (collider == null) return targetPosition;

        Vector3 p1 = startPosition + collider.center + Vector3.up * (collider.height * 0.5f - collider.radius);
        Vector3 p2 = startPosition + collider.center + Vector3.up * (-collider.height * 0.5f + collider.radius);

        RaycastHit hit;
        if(Physics.CapsuleCast(p1, p2, collider.radius, direction, out hit, distance, owner.ObstacleLayer))
        {
            float safeDistance = Mathf.Max(0f, hit.distance - 0.1f);
            targetPosition = startPosition + direction * safeDistance;
        }
        return targetPosition;
    }
}
