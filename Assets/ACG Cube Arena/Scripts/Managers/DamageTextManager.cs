using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DamageText damageTextPrefab;

    [Header("Pooling")]
    private ObjectPool<DamageText> damageTextPool;

    void Awake()
    {
        EnemyStats.onEnemyHit += EnemyHitCallback;
    }
    private void OnDestroy()
    {
        EnemyStats.onEnemyHit -= EnemyHitCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private DamageText CreateFunction()
    {
        return Instantiate(damageTextPrefab);
    }
    private void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }
    private void ActionOnRelease(DamageText damageText)
    {
        if (damageText != null)
        {
            damageText.gameObject.SetActive(false);
        }
    }
    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

    private void EnemyHitCallback(int damage, Vector3 enemyPos, bool isCritical)
    {
        Vector3 spawnPosition = enemyPos + Vector3.up * 3f;
        DamageText damageTextInstance = damageTextPool.Get();
        damageTextInstance.transform.position = spawnPosition;
        damageTextInstance.Animate(damage, isCritical);
        DOVirtual.DelayedCall(0.4f, () => damageTextPool.Release(damageTextInstance));
    }
}
