using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CrimsonAttackManager : MonoBehaviour
{
    public static CrimsonAttackManager instance;



    [Header("Elements")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float chargeDuration;
    [SerializeField] private FinalBossPatternIconSO iconSO;


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


    public void StartCrimsonAttackPattern(float duration, EnemyStats stats)
    {
        GameEventsManager.TriggerFinalBossPatternStarted(iconSO, duration);
        StartCoroutine(CrimsonAttackPatternRoutine(duration, stats));
    }


    private IEnumerator CrimsonAttackPatternRoutine(float duration, EnemyStats stats)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            for (int i = 0; i < 10; i++)
            {
                StartCoroutine(SingleCrimsonAttackRoutine(stats));
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(5f);
        }
        yield return null;
    }

    private IEnumerator SingleCrimsonAttackRoutine(EnemyStats stats)
    {
        // Prepare
        Vector3 targetPosition = GetRandomPositionOnTilemap();

        // Show Indicator
        AoeAttackIndicator aoeAttackIndicatorInstance = VFXPoolManager.instance.enemyAoeIndicatorPool.Get().GetComponent<AoeAttackIndicator>();
        aoeAttackIndicatorInstance.transform.position = targetPosition;
        aoeAttackIndicatorInstance.transform.rotation = Quaternion.identity;
        aoeAttackIndicatorInstance.SetRadius(7f);
        aoeAttackIndicatorInstance.StartExpanding(chargeDuration);
        DOVirtual.DelayedCall(chargeDuration + 0.3f, () => VFXPoolManager.instance.enemyAoeIndicatorPool.Release(aoeAttackIndicatorInstance.gameObject));
        float radius = aoeAttackIndicatorInstance.GetComponentInChildren<MeshRenderer>().bounds.extents.magnitude * 0.6f;
        yield return new WaitForSeconds(chargeDuration);

        //Spawn VFX
        GameObject aoeVFXInstance = VFXPoolManager.instance.crimsonAoeVFXPool.Get();
        aoeVFXInstance.transform.position = targetPosition;
        aoeVFXInstance.transform.localScale = new Vector3(1, 1, 1);
        DOVirtual.DelayedCall(2f, () => VFXPoolManager.instance.crimsonAoeVFXPool.Release(aoeVFXInstance));

        yield return new WaitForSeconds(0.3f);

        // AudioManager.instance.PlayMageAttackSound();

        Collider[] colliders = Physics.OverlapSphere(targetPosition, radius);
        DrawDebugCircle(targetPosition, radius, 360, Color.green, 5f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                collider.gameObject.GetComponent<PlayerStats>().TakeDamage((int)stats.AttackDamage.GetValue());
            }
        }

    }

    public Vector3 GetRandomPositionOnTilemap()
    {
        Bounds bounds = Helper.CalculateBoundsFromColliders(tilemap.transform);
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 worldPosition = new Vector3(x, 0.7f, z);
        return worldPosition;
    }

    void DrawDebugCircle(Vector3 center, float radius, int segments, Color color, float duration)
    {
        float angle = 0f;
        float angleStep = 360f / segments;

        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0) * radius, 0, Mathf.Sin(0) * radius);

        for (int i = 1; i <= segments; i++)
        {
            angle += angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 nextPoint = center + new Vector3(Mathf.Cos(rad) * radius, 0, Mathf.Sin(rad) * radius);
            Debug.DrawLine(prevPoint, nextPoint, color, duration);

            prevPoint = nextPoint;
        }
    }
    

}
