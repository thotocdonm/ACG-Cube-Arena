using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IchigoAttackingState : PlayerBaseState
{

    private bool isExiting;

    public IchigoAttackingState(PlayerController owner, StateMachine stateMachine) : base(owner, stateMachine) { }

    public override void Enter()
    {
        isExiting = false;
    }


    public override void HandleAttack()
    {

        if (owner.IchigoComboAttack.IsAttacking)
        {
            owner.IchigoComboAttack.BufferNextAttack();
        } else
        {
            owner.IchigoComboAttack.TryStartAttack();
        }
    }

    public override void Update()
    {

        if (isExiting) return;

        if (owner.IsAttackHeld)
        {
            HandleAttack();
        }

        if (!owner.IchigoComboAttack.IsAttacking && owner.IchigoComboAttack.IsComboExpired())
        {
            Vector2 currentInput = owner.LastMoveInput;

            if (currentInput.magnitude > 0.1f)
            {
                owner.ChangeState(owner.movingState);
            }
            else
            {
                owner.ChangeState(owner.idleState);
            }
        }
    }

    public override void HandleDash()
    {
        if(isExiting) return;

        if (owner.CanDash())
        {
            isExiting = true;
            owner.IchigoComboAttack.CancelCurrentAttack();
            owner.ResetDashCooldown();
            owner.ChangeState(owner.dashingState);
        }
    }
}
