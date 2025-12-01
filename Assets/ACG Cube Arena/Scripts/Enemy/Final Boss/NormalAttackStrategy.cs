using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NormalAttackStrategy : IAttackStrategy
{
    private readonly Enemy owner;
    private readonly Rigidbody rb;
    private readonly Animator animator;
    private readonly Transform playerTarget;
    private readonly EnemyStats stats;
    private readonly GameObject chargingVFXPrefab;
    private readonly Transform normalAttackProjectileSpawnPoint;

    public NormalAttackStrategy(Enemy owner, Rigidbody rb, Animator animator, Transform playerTarget, EnemyStats stats, GameObject chargingVFXPrefab, Transform normalAttackProjectileSpawnPoint)
    {
        this.owner = owner;
        this.rb = rb;
        this.animator = animator;
        this.playerTarget = playerTarget;
        this.stats = stats;
        this.chargingVFXPrefab = chargingVFXPrefab;
        this.normalAttackProjectileSpawnPoint = normalAttackProjectileSpawnPoint;

    }

    public void Execute(Action onComplete)
    {
        owner.StartCoroutine(NormalAttackRoutine(onComplete));
        
    }

    private IEnumerator NormalAttackRoutine(Action onComplete)
    {
        Vector3 directionToPlayer = (playerTarget.position - owner.transform.position).normalized;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);


        //Charging
        chargingVFXPrefab.SetActive(true);
        yield return new WaitForSeconds(1f);

        chargingVFXPrefab.SetActive(false);

        //Spawn Normal Attack Projectile
        for (int i = 0; i < 3; i++)
        {
            GameObject normalAttackProjectileInstance = VFXPoolManager.instance.normalAttackProjectilePool.Get();
            normalAttackProjectileInstance.transform.position = normalAttackProjectileSpawnPoint.position;
            normalAttackProjectileInstance.transform.rotation = targetRotation;
            NormalAttackProjectile normalAttackProjectile = normalAttackProjectileInstance.GetComponent<NormalAttackProjectile>();
            normalAttackProjectile.Initialize((int)stats.AttackDamage.GetValue());
            normalAttackProjectile.Fire();
            DOVirtual.DelayedCall(2f, () => VFXPoolManager.instance.normalAttackProjectilePool.Release(normalAttackProjectileInstance));
            yield return new WaitForSeconds(0.5f);
        }

        onComplete?.Invoke();

    }
}
