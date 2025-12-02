using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ThunderAttackManager : MonoBehaviour
{
    public static ThunderAttackManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }


    public void StartThunderAttackPattern(float duration, EnemyStats stats, Transform spawnPoint)
    {
        ThunderAttackPatternRoutine(duration, stats, spawnPoint);
    }


    private void ThunderAttackPatternRoutine(float duration, EnemyStats stats, Transform spawnPoint)
    {
        for (int i = 0; i < 3; i++)
        {
            Quaternion randomYRot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            SingleThunderAttackRoutine(stats, randomYRot, duration, spawnPoint);
        }

    }

    private void SingleThunderAttackRoutine(EnemyStats stats, Quaternion rotation, float duration, Transform spawnPoint)
    {

        //Spawn VFX
        GameObject thunderAttackProjectileInstance = VFXPoolManager.instance.thunderAttackProjectilePool.Get();
        thunderAttackProjectileInstance.transform.rotation = rotation;
        thunderAttackProjectileInstance.transform.position = spawnPoint.position;
        thunderAttackProjectileInstance.transform.localScale = new Vector3(1, 1, 1);
        ThunderAttackProjectile thunderAttackProjectile = thunderAttackProjectileInstance.GetComponent<ThunderAttackProjectile>();
        thunderAttackProjectile.Initialize((int)stats.AttackDamage.GetValue());
        thunderAttackProjectile.Fire();
        DOVirtual.DelayedCall(duration, () => VFXPoolManager.instance.thunderAttackProjectilePool.Release(thunderAttackProjectileInstance));

    }




}
