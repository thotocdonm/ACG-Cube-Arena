using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State <T> : IState where T : class
{
    protected readonly T owner;
    protected readonly StateMachine stateMachine;

    public State(T owner, StateMachine stateMachine)
    {
        this.owner = owner;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter(){}
    public virtual void Update(){}
    public virtual void FixedUpdate(){}
    public virtual void Exit(){}
}
