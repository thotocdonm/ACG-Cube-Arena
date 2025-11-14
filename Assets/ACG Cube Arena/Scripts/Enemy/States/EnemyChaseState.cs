using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(Enemy owner, StateMachine stateMachine) : base(owner, stateMachine) { }

    public override void Update()
    {
        float distanceToPlayer = Vector3.Distance(owner.transform.position, owner.PlayerTarget.position);
        if (distanceToPlayer <= owner.GetEnemyStats().DetectionRange.GetValue() && owner.IsAttackReady())
        {
            stateMachine.ChangeState(owner.EnemyAttackState);
        }
    }

    public override void FixedUpdate()
    {
        Vector3 directionToPlayer = (owner.PlayerTarget.position - owner.transform.position).normalized;
        rb.MovePosition(owner.transform.position + directionToPlayer * owner.GetEnemyStats().MoveSpeed.GetValue() * Time.fixedDeltaTime);

        if(directionToPlayer != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, toRotation, 500f * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }
    }
}
