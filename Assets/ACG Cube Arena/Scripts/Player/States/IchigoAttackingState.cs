using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IchigoAttackingState : PlayerBaseState
{
    public IchigoAttackingState(PlayerController owner, StateMachine stateMachine) : base(owner, stateMachine) { }

    public override void Enter()
    {
        if (!owner.IchigoComboAttack.TryStartAttack())
        {
            owner.ChangeState(owner.idleState);
        }
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
        if(!owner.IchigoComboAttack.IsAttacking && owner.IchigoComboAttack.IsComboExpired())
        {
            Vector2 currentInput = owner.LastMoveInput;

            if (currentInput.magnitude > 0.1f)
            {
                owner.movingState.setMoveInput(currentInput);
                owner.ChangeState(owner.movingState);
            }
            else
            {
                owner.ChangeState(owner.idleState);
            }
        }
    }
}
