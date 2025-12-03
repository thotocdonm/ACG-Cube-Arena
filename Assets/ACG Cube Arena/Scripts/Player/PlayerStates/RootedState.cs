using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootedState : PlayerBaseState
{
    public RootedState(PlayerController owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void Enter()
    {
        owner.Animator.SetBool("IsMoving", false);
        owner.RootedVFX.SetActive(true);
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
    }
    
    public override void Exit()
    {
        owner.RootedVFX.SetActive(false);
    }

    public override void HandleMove(Vector2 Input)
    {

    }

    public override void HandleDash()
    {

    }
    
    public override void HandleAttack()
    {

    }
    
}
