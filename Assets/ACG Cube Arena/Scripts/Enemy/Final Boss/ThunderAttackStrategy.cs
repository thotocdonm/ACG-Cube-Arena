using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttackStrategy : IAttackStrategy
{
    private readonly Enemy owner;
    private readonly Rigidbody rb;
    private readonly Animator animator;
    private readonly Transform playerTarget;
    private readonly EnemyStats stats;

    public ThunderAttackStrategy(Enemy owner, Rigidbody rb, Animator animator, Transform playerTarget, EnemyStats stats)
    {
        this.owner = owner;
        this.rb = rb;
        this.animator = animator;
        this.playerTarget = playerTarget;
        this.stats = stats;
    }

    public void Execute(Action onComplete)
    {
        owner.StartCoroutine(ThunderAttackRoutine(onComplete));
    }

    private IEnumerator ThunderAttackRoutine(Action onComplete)
    {
        yield return new WaitForSeconds(3f);
        ThunderAttackManager.instance.StartThunderAttackPattern(60f, stats, owner.transform);
        onComplete?.Invoke();
    }

}
