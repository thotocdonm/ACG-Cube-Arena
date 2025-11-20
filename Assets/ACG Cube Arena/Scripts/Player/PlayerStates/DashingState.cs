using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DashingState : PlayerBaseState
{

    private Vector3 dashDirection;
    private Tween dashTween;

    public DashingState(PlayerController owner, StateMachine stateMachine) : base(owner, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Dashing State");
        owner.Animator.SetTrigger("Dash");

        if (owner.LastMoveInput.magnitude < 0.1f)
        {
            dashDirection = owner.transform.forward;
        }
        else
        {
            dashDirection = new Vector3(owner.LastMoveInput.x, 0, owner.LastMoveInput.y);
        }
        Vector3 targetPosition = owner.transform.position + dashDirection * owner.DashSpeed;

        dashTween = rb.DOMove(targetPosition, owner.DashDuration).SetUpdate(UpdateType.Fixed).SetEase(Ease.OutCubic).OnComplete(OnDashComplete);
    }

    public override void Exit()
    {
        dashTween?.Kill();
    }

    private void OnDashComplete()
    {
        owner.Animator.ResetTrigger("Dash");
        Debug.LogWarning("Dash Complete");
        owner.ResetDashCooldown();

        Vector2 currentInput = owner.LastMoveInput;
        Debug.LogWarning("Current Input: " + currentInput);

        if (currentInput.magnitude > 0.1f)
        {
            owner.ChangeState(owner.movingState);
        }
        else
        {
            owner.ChangeState(owner.idleState);
        }
    }

    public override void HandleMove(Vector2 Input) { }

    public override void HandleAttack()
    {
        Debug.Log("Handle Attack in Dashing State");
    }

}
