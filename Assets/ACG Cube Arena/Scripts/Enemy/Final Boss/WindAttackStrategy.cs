using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAttackStrategy : IAttackStrategy
{
    private readonly Enemy owner;
    private readonly Rigidbody rb;
    private readonly Animator animator;
    private readonly Transform playerTarget;
    private readonly EnemyStats stats;

    public WindAttackStrategy(Enemy owner, Rigidbody rb, Animator animator, Transform playerTarget, EnemyStats stats)
    {
        this.owner = owner;
        this.rb = rb;
        this.animator = animator;
        this.playerTarget = playerTarget;
        this.stats = stats;
    }

    public void Execute(Action onComplete)
    {
        owner.StartCoroutine(WindAttackRoutine(onComplete));
    }

    private IEnumerator WindAttackRoutine(Action onComplete)
    {
        yield return new WaitForSeconds(3f);
        WindAttackManager.instance.StartWindAttackPattern(15f, stats);
        onComplete?.Invoke();
    }
}
