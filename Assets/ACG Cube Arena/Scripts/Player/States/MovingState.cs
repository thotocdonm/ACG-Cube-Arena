using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : PlayerBaseState
{
    private Vector2 moveInput;

    public MovingState(PlayerController player, Rigidbody rb) : base(player, rb)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter Moving State");
    }

    public override void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 newVelocity = moveDirection * player.MoveSpeed;
        rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);

        if(moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, toRotation, player.RotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }
    }

    public override void HandleMove(Vector2 input){
        moveInput = input;
        if(input.magnitude < 0.1f)
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void HandleDash(){
        if(player.CanDash())
        {
            player.ChangeState(player.dashingState);
        }
    }   

    public override void HandleAttack(){
        Debug.Log("Handle Attack in Moving State");
    }
}
