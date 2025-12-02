using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LightAttackManager : MonoBehaviour
{
    public static LightAttackManager instance;



    [Header("Elements")]
    [SerializeField] private float chargeDuration;

    private Transform playerTarget;


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
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
    }


    public void StartLightAttackPattern(float duration, EnemyStats stats)
    {
        StartCoroutine(LightAttackPatternRoutine(duration, stats));
    }


    private IEnumerator LightAttackPatternRoutine(float duration, EnemyStats stats)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            StartCoroutine(SingleLightAttackRoutine(stats));
            yield return new WaitForSeconds(5f);
        }
        yield return null;
    }

    private IEnumerator SingleLightAttackRoutine(EnemyStats stats)
    {
        // Prepare


        // Show Indicator
        AoeAttackIndicator aoeAttackIndicatorInstance = VFXPoolManager.instance.enemyAoeIndicatorPool.Get().GetComponent<AoeAttackIndicator>();
        aoeAttackIndicatorInstance.transform.rotation = Quaternion.identity;
        aoeAttackIndicatorInstance.SetRadius(7f);
        aoeAttackIndicatorInstance.StartTrackingThenLock(chargeDuration * 0.75f, chargeDuration);
        DOVirtual.DelayedCall(chargeDuration + 0.3f, () => VFXPoolManager.instance.enemyAoeIndicatorPool.Release(aoeAttackIndicatorInstance.gameObject));
        float radius = aoeAttackIndicatorInstance.GetComponentInChildren<MeshRenderer>().bounds.extents.magnitude * 0.6f;
        yield return new WaitForSeconds(chargeDuration);

        //Spawn VFX
        Vector3 targetPosition = aoeAttackIndicatorInstance.transform.position;
        GameObject aoeVFXInstance = VFXPoolManager.instance.lightAoeVFXPool.Get();
        aoeVFXInstance.transform.position = targetPosition;
        aoeVFXInstance.transform.localScale = new Vector3(1, 1, 1);
        DOVirtual.DelayedCall(0.7f, () => VFXPoolManager.instance.lightAoeVFXPool.Release(aoeVFXInstance));

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
