using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState 
{
    protected PlayerController player;
    protected Rigidbody rb;

    public PlayerBaseState(PlayerController player, Rigidbody rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }

    public virtual void HandleMove(Vector2 Input) { }
    public virtual void HandleDash() { }
    public virtual void HandleAttack() { }
}
