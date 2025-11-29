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
        owner.Animator.SetTrigger("Dash");

        if (owner.LastMoveInput.magnitude < 0.1f)
        {
            dashDirection = owner.transform.forward;
            dashDirection.y = 0;
            dashDirection.Normalize();
        }
        else
        {
            dashDirection = new Vector3(owner.LastMoveInput.x, 0, owner.LastMoveInput.y);
        }
        Vector3 targetPosition = GetSafeTargetPosition(dashDirection, owner.DashDistance);

        dashTween = rb.DOMove(targetPosition, owner.DashDuration).SetUpdate(UpdateType.Fixed).SetEase(Ease.OutCubic).OnComplete(OnDashComplete);
    }

    public override void Exit()
    {
        dashTween?.Kill();
    }

    private void OnDashComplete()
    {
        owner.Animator.ResetTrigger("Dash");
        owner.ResetDashCooldown();

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

    private Vector3 GetSafeTargetPosition(Vector3 direction, float distance)
    {
        Vector3 startPosition = owner.transform.position - direction * 0.1f;
        Vector3 targetPosition = startPosition + direction * distance;

        Debug.DrawRay(startPosition, direction * distance, Color.red, 2f);


        CapsuleCollider collider = owner.GetComponent<CapsuleCollider>();
        if (collider == null) return targetPosition;

        Vector3 p1 = startPosition + collider.center + Vector3.up * (collider.height * 0.5f - collider.radius);
        Vector3 p2 = startPosition + collider.center + Vector3.up * (-collider.height * 0.5f + collider.radius);

        RaycastHit hit;
        if (Physics.CapsuleCast(p1, p2, collider.radius, direction, out hit, distance, owner.ObstacleLayer, QueryTriggerInteraction.Ignore))
        {
            float safeDistance = Mathf.Max(0f, hit.distance - 0.1f);
            targetPosition = startPosition + direction * safeDistance;
        }
        return targetPosition;
    }
    

    public override void HandleMove(Vector2 Input) { }

    public override void HandleAttack()
    {
        
    }

}
