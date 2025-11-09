using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DashingState : PlayerBaseState
{

    private Vector3 dashDirection;
    private Tween dashTween;

    public DashingState(PlayerController player, Rigidbody rb) : base(player, rb) { }

    public override void Enter()
    {
        Debug.Log("Enter Dashing State");

        player.Animator.Play("Dashing");

        if (player.LastMoveInput.magnitude < 0.1f)
        {
            dashDirection = player.transform.forward;
        }
        else
        {
            dashDirection = new Vector3(player.LastMoveInput.x, 0, player.LastMoveInput.y);
        }

        Vector3 targetPosition = player.transform.position + dashDirection * player.DashSpeed;
        dashTween = rb.DOMove(targetPosition, player.DashDuration).SetEase(Ease.OutCubic).OnComplete(OnDashComplete);
    }

    public override void Exit()
    {
        dashTween?.Kill();
    }

    private void OnDashComplete()
    {
        Debug.Log("Dash Complete");

        Vector2 currentInput = player.LastMoveInput;

        if (currentInput.magnitude > 0.1f)
        {
            player.movingState.setMoveInput(currentInput);
            player.ChangeState(player.movingState);
        }
        else
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void HandleMove(Vector2 Input) { }

}
