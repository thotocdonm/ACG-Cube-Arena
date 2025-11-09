using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using DG.Tweening;

public class MeleeEnemy : Enemy
{
    [Header("Charge Attack Settings")]
    [SerializeField] private LineRenderer chargeIndicator;
    [SerializeField] private float telegraphDuration;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float recoveryDuration;

    private bool isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        if(chargeIndicator != null)
        {
            chargeIndicator.enabled = false;
        }
        
    }

    protected override void Attack()
    {
        if (isAttacking) return;

        StartCoroutine(ChargeAttackSequence());
    }

    private IEnumerator ChargeAttackSequence()
    {
        // Prepare to attack
        isAttacking = true;
        StopMovement();

        Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;

        // Show charge indicator
        if (chargeIndicator != null)
        {
            chargeIndicator.enabled = true;
            chargeIndicator.SetPosition(0, Vector3.zero);
            chargeIndicator.SetPosition(1, directionToPlayer * stats.attackRange * 1.5f);
        }

        yield return new WaitForSeconds(telegraphDuration);

        // Charging toward player
        if (chargeIndicator != null)
        {
            chargeIndicator.enabled = false;
        }

        Vector3 targetPosition = transform.position + directionToPlayer * stats.attackRange * 1.5f;

        rb.DOMove(targetPosition, chargeDuration).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            Debug.Log("Charged");
        });
        
        // Recovery
        StopMovement();

        yield return new WaitForSeconds(recoveryDuration);
        isAttacking = false;
        ResumeMovement();

        

        
    }
}
