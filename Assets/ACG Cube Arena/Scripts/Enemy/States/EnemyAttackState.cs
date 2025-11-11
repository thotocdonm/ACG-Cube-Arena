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
        attackCooldownTimer = owner.GetEnemyStats().AttackCooldown.GetValue();
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
        if (distanceToPlayer > owner.GetEnemyStats().DetectionRange.GetValue())
        {
            stateMachine.ChangeState(owner.EnemyChaseState);
            return;
        }
        
        if(!isAttacking && attackCooldownTimer >= owner.GetEnemyStats().AttackCooldown.GetValue())
        {
            isAttacking = true;
            owner.AttackStrategy.Execute(() =>
            {
                attackCooldownTimer = 0f;
                isAttacking = false;
                stateMachine.ChangeState(owner.EnemyChaseState);
            });
        }
    }

  
}
