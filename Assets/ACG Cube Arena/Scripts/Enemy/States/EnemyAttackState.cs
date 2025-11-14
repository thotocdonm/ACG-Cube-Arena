using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{


    public EnemyAttackState(Enemy owner, StateMachine stateMachine) : base(owner, stateMachine) { }

    public override void Enter()
    {
       rb.velocity = Vector3.zero;
       owner.AttackStrategy.Execute(() =>
       {
            owner.SetAttackCooldown(owner.GetEnemyStats().AttackCooldown.GetValue());
            stateMachine.ChangeState(owner.EnemyChaseState);
       });
    }

    public override void Update()
    {
        
    }
    


  
}
