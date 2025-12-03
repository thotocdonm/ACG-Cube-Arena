using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttackStrategy : IAttackStrategy
{
    private readonly Enemy owner;
    private readonly Rigidbody rb;
    private readonly Animator animator;
    private readonly Transform playerTarget;
    private readonly EnemyStats stats;

    public RootAttackStrategy(Enemy owner, Rigidbody rb, Animator animator, Transform playerTarget, EnemyStats stats)
    {
        this.owner = owner;
        this.rb = rb;
        this.animator = animator;
        this.playerTarget = playerTarget;
        this.stats = stats;
    }

    public void Execute(Action onComplete)
    {
        owner.StartCoroutine(RootAttackRoutine(onComplete));
    }

    private IEnumerator RootAttackRoutine(Action onComplete)
    {
        yield return new WaitForSeconds(3f);
        RootAttackManager.instance.ActiveRootWindow(stats);
        onComplete?.Invoke();
    }
}
