using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyHitFeedback : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject hitVFX;

    // Start is called before the first frame update
    void Start()
    {
        EnemyStats.onEnemyHit += EnemyHitCallback;
    }
    private void OnDestroy()
    {
        EnemyStats.onEnemyHit -= EnemyHitCallback;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void EnemyHitCallback(int damage, Vector3 enemyPos, bool isCritical, Vector3 hitPoint)
    {
        
        Quaternion rotation = Quaternion.Euler(0,Random.Range(0,360),0);
        GameObject vfxInstance = VFXPoolManager.instance.enemyHittedVFXPool.Get();
        vfxInstance.transform.position = hitPoint;
        vfxInstance.transform.rotation = rotation;
        DOVirtual.DelayedCall(1f, () => VFXPoolManager.instance.enemyHittedVFXPool.Release(vfxInstance));
    }
}
