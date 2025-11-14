using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("Enemy hit feedback");
        Quaternion rotation = Quaternion.Euler(0,Random.Range(0,360),0);
        GameObject vfxInstance = Instantiate(hitVFX, hitPoint, rotation);
        Destroy(vfxInstance, 1f);
    }
}
