using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager instance;

    private Coroutine hitStopCoroutine;
    private float originalTimeScale;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        originalTimeScale = Time.timeScale;
        
    }
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
        DoHitStop(0.05f, 0.05f);
    }

    public void DoHitStop(float duration, float timeScale)
    {
        if(hitStopCoroutine != null)
        {
            StopCoroutine(hitStopCoroutine);
        }
        hitStopCoroutine = StartCoroutine(HitStopCoroutine(duration, timeScale));
    }
    
    private IEnumerator HitStopCoroutine(float duration, float timeScale)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = originalTimeScale;
        hitStopCoroutine = null;
    }
}
