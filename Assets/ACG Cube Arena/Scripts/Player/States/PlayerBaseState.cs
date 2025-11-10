using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State<PlayerController>
{
    protected readonly Rigidbody rb;

    public PlayerBaseState(PlayerController owner, StateMachine stateMachine) : base(owner, stateMachine)
    {

       rb = owner.Rigidbody;
    }

    public virtual void HandleMove(Vector2 Input) { }
    public virtual void HandleDash() { }
    public virtual void HandleAttack() { }
}
