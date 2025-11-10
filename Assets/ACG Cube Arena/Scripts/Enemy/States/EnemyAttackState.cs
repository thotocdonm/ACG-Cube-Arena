using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private bool isAttacking;
    private float attackCooldownTimer;

    public EnemyAttackState(Enemy owner, StateMachine stateMachine) : base(owner, stateMachine) { }

    public override void Enter()
    {
        isAttacking = false;
        attackCooldownTimer = owner.Stats.attackCooldown;
        rb.velocity = Vector3.zero;
        Debug.Log("Enter Enemy Attack State");
    }

    public override void Update()
    {
        if (isAttacking)
        {
            return;
        }

        attackCooldownTimer += Time.deltaTime;
        
        float distanceToPlayer = Vector3.Distance(owner.transform.position, owner.PlayerTarget.position);
        if (distanceToPlayer > owner.Stats.detectionRange)
        {
            stateMachine.ChangeState(owner.EnemyChaseState);
            return;
        }
        
        if(!isAttacking && attackCooldownTimer >= owner.Stats.attackCooldown)
        {
            isAttacking = true;
            owner.AttackStrategy.Execute(() =>
            {
                attackCooldownTimer = 0f;
                isAttacking = false;
            });
        }
    }

  
}
