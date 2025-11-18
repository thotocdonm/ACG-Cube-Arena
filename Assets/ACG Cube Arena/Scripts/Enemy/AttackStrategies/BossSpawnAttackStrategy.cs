using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnAttackStrategy : IAttackStrategy
{
    private readonly Enemy owner;
    private readonly Rigidbody rb;
    private readonly Animator animator;
    private readonly Transform playerTarget;
    private readonly EnemyStats stats;
    private readonly ParticleSystem chargingVFX;
    private readonly ParticleSystem sparkVFX;
    private readonly AoeAttackIndicator aoeAttackIndicator;
    private readonly GameObject aoeVFXPrefab;
    private readonly GameObject[] spawnableEnemies;
    private readonly GameObject spawnIndicator;
    private readonly float chargeDuration;
    private readonly float recoveryDuration;

    private readonly int numberOfSpawn = 5;
    private readonly float spawnInterval = 0.3f;

    public BossSpawnAttackStrategy(Enemy owner, Rigidbody rb, Animator animator, GameObject[] spawnableEnemies, GameObject spawnIndicator, Transform playerTarget, EnemyStats stats, ParticleSystem chargingVFX, ParticleSystem sparkVFX, AoeAttackIndicator aoeAttackIndicator, GameObject aoeVFXPrefab, float chargeDuration, float recoveryDuration)
    {
        this.owner = owner;
        this.rb = rb;
        this.animator = animator;
        this.spawnableEnemies = spawnableEnemies;
        this.spawnIndicator = spawnIndicator;
        this.playerTarget = playerTarget;
        this.stats = stats; 
        this.chargingVFX = chargingVFX;
        this.sparkVFX = sparkVFX;
        this.aoeAttackIndicator = aoeAttackIndicator;
        this.aoeVFXPrefab = aoeVFXPrefab;
        this.chargeDuration = chargeDuration;
        this.recoveryDuration = recoveryDuration;
    }

    public void Execute(Action onComplete)
    {
        owner.StartCoroutine(BossSpawnAttackSequence(onComplete));
    }

    private IEnumerator BossSpawnAttackSequence(Action onComplete)
    {
        EnableVFX();
        animator.Play("SpawnMobs");

        for (int i = 0; i < numberOfSpawn; i++)
        {
            owner.StartCoroutine(SingleSpawnRoutine());
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(chargeDuration - (numberOfSpawn - 1) * spawnInterval);

        DisableVFX();
        animator.Play("Idle");

        yield return new WaitForSeconds(Mathf.Max(0, recoveryDuration));

        onComplete?.Invoke();
    }
    
    private IEnumerator SingleSpawnRoutine()
    {
        // Prepare
        Vector3 playerPosition = playerTarget.position;

        Vector2 randomOffset2D = UnityEngine.Random.insideUnitCircle * 20;
        Vector3 randomOffset = new Vector3(randomOffset2D.x, 0, randomOffset2D.y);

        Vector3 targetPosition = playerPosition + randomOffset;
        targetPosition.y = 0.7f;

        // Show Indicator
        GameObject spawnIndicatorInstance = VFXPoolManager.instance.enemySpawnVFXPool.Get();
        spawnIndicatorInstance.transform.position = targetPosition;
        spawnIndicatorInstance.transform.rotation = Quaternion.identity;
        spawnIndicatorInstance.transform.localRotation = Quaternion.Euler(90, 0, 0);
        GameObject.Destroy(spawnIndicatorInstance.gameObject, chargeDuration + 0.3f);
        yield return new WaitForSeconds(chargeDuration);

        //Spawn Enemy
        GameObject enemyInstance = GameObject.Instantiate(spawnableEnemies[UnityEngine.Random.Range(0, spawnableEnemies.Length)], targetPosition, Quaternion.identity);

        AudioManager.instance.PlayEnemySpawnSound();
        
    }

    private void DisableVFX()
    {
        sparkVFX.Stop();
        sparkVFX.gameObject.SetActive(false);
        chargingVFX.Stop();
        chargingVFX.gameObject.SetActive(false);
    }
    
    private void EnableVFX()
    {
        sparkVFX.gameObject.SetActive(true);
        sparkVFX.Play();
        chargingVFX.gameObject.SetActive(true);
        chargingVFX.Play();
    }
}
