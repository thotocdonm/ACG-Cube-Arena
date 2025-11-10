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

        owner.Animator.Play("Dashing");

        if (owner.LastMoveInput.magnitude < 0.1f)
        {
            dashDirection = owner.transform.forward;
        }
        else
        {
            dashDirection = new Vector3(owner.LastMoveInput.x, 0, owner.LastMoveInput.y);
        }

        Vector3 targetPosition = owner.transform.position + dashDirection * owner.DashSpeed;
        dashTween = rb.DOMove(targetPosition, owner.DashDuration).SetEase(Ease.OutCubic).OnComplete(OnDashComplete);
    }

    public override void Exit()
    {
        dashTween?.Kill();
    }

    private void OnDashComplete()
    {
        Debug.Log("Dash Complete");

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

    public override void HandleMove(Vector2 Input) { }

}
