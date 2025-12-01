using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonAttackStrategy : IAttackStrategy
{
    private readonly Enemy owner;
    private readonly Rigidbody rb;
    private readonly Animator animator;
    private readonly Transform playerTarget;
    private readonly EnemyStats stats;

    public CrimsonAttackStrategy(Enemy owner, Rigidbody rb, Animator animator, Transform playerTarget, EnemyStats stats)
    {
        this.owner = owner;
        this.rb = rb;
        this.animator = animator;
        this.playerTarget = playerTarget;
        this.stats = stats;
    }

    public void Execute(Action onComplete)
    {
        CrimsonAttackManager.instance.StartCrimsonAttackPattern(60f, stats);
        onComplete?.Invoke();
    }

}
