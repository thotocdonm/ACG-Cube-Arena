using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerController player, Rigidbody rb) : base(player, rb)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter Idle State");
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    public override void HandleMove(Vector2 Input)
    {
        if(Input.magnitude > 0.1f)
        {
            player.ChangeState(player.movingState);
        }
    }

    public override void HandleDash()
    {
        if(player.CanDash())
        {
            player.ChangeState(player.dashingState);
        }
    }
    
    public override void HandleAttack()
    {
        
    }
    
}
