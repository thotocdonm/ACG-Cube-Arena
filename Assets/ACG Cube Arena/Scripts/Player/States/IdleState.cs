using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerController owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter Idle State");
        owner.Animator.Play("Idle");
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    public override void HandleMove(Vector2 Input)
    {
        if(Input.magnitude > 0.1f)
        {
            owner.ChangeState(owner.movingState);
        }
    }

    public override void HandleDash()
    {
        if(owner.CanDash())
        {
            owner.ChangeState(owner.dashingState);
        }
    }
    
    public override void HandleAttack()
    {
        
    }
    
}
