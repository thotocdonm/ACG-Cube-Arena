using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IchigoChargingState : PlayerBaseState
{
    private Vector2 moveInput;

    public IchigoChargingState(PlayerController owner, StateMachine stateMachine) : base(owner, stateMachine) { }

    public void setMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public override void Enter()
    {
        owner.IchigoSkillAttack.StartCharging();
        HandleMove(owner.LastMoveInput);
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 newVelocity = moveDirection * owner.MoveSpeed;
        rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);

        if (owner.AimDirection.sqrMagnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(owner.AimDirection, Vector3.up);
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, toRotation, owner.RotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }
    }

    public override void Exit()
    {
        
    }

    public override void HandleMove(Vector2 input)
    {
        setMoveInput(input);
    }

    public override void HandleDash()
    {
        owner.IchigoSkillAttack.CancelSkill();
        owner.ChangeState(owner.dashingState);
    }
}
