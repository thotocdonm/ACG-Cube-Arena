using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : State<Enemy>
{
    protected readonly Rigidbody rb;
    

    public EnemyBaseState(Enemy owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
       rb = owner.Rigidbody;
    }

    
}
