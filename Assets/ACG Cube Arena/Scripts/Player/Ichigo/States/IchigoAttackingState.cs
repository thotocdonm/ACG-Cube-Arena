using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IchigoAttackingState : PlayerBaseState
{

    private bool canBuffer;

    public IchigoAttackingState(PlayerController owner, StateMachine stateMachine) : base(owner, stateMachine) { }


    public override void Enter()
    {
        base.Enter();
        canBuffer = false;

        if (owner.IchigoComboAttack.TryStartAttack())
        {
            owner.StartCoroutine(AllowBufferAfterDelay(0.1f));
        }
        else
        {
            owner.ChangeState(owner.idleState);
        }
    }
    
    private IEnumerator AllowBufferAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canBuffer = true;
    }


    public override void HandleAttack()
    {

        if (owner.IchigoComboAttack.IsAttacking && canBuffer)
        {
            owner.IchigoComboAttack.BufferNextAttack();
        } else
        {
            owner.IchigoComboAttack.TryStartAttack();
        }
    }

    public override void Update()
    {
        if (owner.IchigoComboAttack.IsComboExpired())
        {
            if (!owner.IchigoComboAttack.IsAttacking)
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

    }
    
    public override void FixedUpdate()
    {
        Vector3 baseVelocity = new Vector3(0, 0, 0);
        Vector3 final = baseVelocity + owner.ExternalVelocity * 3f;
        rb.velocity = new Vector3(final.x, rb.velocity.y, final.z);
    }

    public override void HandleDash()
    {
        if (owner.CanDash())
        {
            owner.IchigoComboAttack.CancelCurrentAttack();
            owner.ResetDashCooldown();
            owner.ChangeState(owner.dashingState);
        }
    }
}
