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
        owner.Animator.SetBool("IsMoving", false);
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    public override void Update()
    {
        Debug.Log("Idle State Update");
        Debug.Log("Aim Direction: " + owner.AimDirection);
        if(owner.AimDirection.sqrMagnitude > 0.01f)
        {
            Debug.Log("Aiming");
            Quaternion toRotation = Quaternion.LookRotation(owner.AimDirection, Vector3.up);
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, toRotation, owner.RotationSpeed * Time.deltaTime);
            rb.MoveRotation(newRotation);
        }
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
