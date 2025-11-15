using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : PlayerBaseState
{
    private Vector2 moveInput;

    public MovingState(PlayerController owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public void setMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public override void Enter()
    {
        owner.Animator.SetBool("IsMoving", true);
        HandleMove(owner.LastMoveInput);
    }

    public override void Exit()
    {
        owner.Animator.SetBool("IsMoving", false);
    }

    public override void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 newVelocity = moveDirection * owner.MoveSpeed;
        rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);

        if(moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, toRotation, owner.RotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }
    }

    public override void HandleMove(Vector2 input){
        setMoveInput(input);
        if(input.magnitude < 0.1f)
        {
            owner.ChangeState(owner.idleState);
        }
    }

    public override void HandleDash(){
        if(owner.CanDash())
        {
            owner.ChangeState(owner.dashingState);
        }
    }   

    public override void HandleAttack(){
        owner.ChangeState(owner.ichigoAttackingState);
    }
    
}
