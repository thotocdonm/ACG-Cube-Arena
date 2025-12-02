using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttackIndicator : MonoBehaviour
{
    [SerializeField] private Transform fillTransform;

    [Header("Tracking")]
    [SerializeField] private float followSmoothing = 20f;
    [SerializeField] private float yHeight = 0.7f;

    private Transform playerTarget;
    private float trackingEndTime;
    private bool isTracking;
    private Coroutine expandCoroutine;

    void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
    }

    private void Update()
    {
        if (!isTracking) return;

        if (Time.time < trackingEndTime && playerTarget != null)
        {
            Vector3 targetPosition = playerTarget.position;
            targetPosition.y = yHeight;
            float k = 1f - Mathf.Exp(-followSmoothing * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, k);
        }
        else
        {
            isTracking = false;
        }
    }
    
    public void StartTrackingThenLock(float trackDuration, float totalDuration)
    {
        trackingEndTime = Time.time + trackDuration;
        isTracking = true;
        if(expandCoroutine != null){
            StopCoroutine(expandCoroutine);
        }
        expandCoroutine = StartCoroutine(ExpandCoroutine(totalDuration));
    }

    public void StartExpanding(float duration)
    {
        if (expandCoroutine != null)
        {
            StopCoroutine(expandCoroutine);
        }
        expandCoroutine = StartCoroutine(ExpandCoroutine(duration));
    }

    private IEnumerator ExpandCoroutine(float duration)
    {
        fillTransform.localScale = Vector3.zero;
        float elapsed = 0;
        Vector3 initialScale = fillTransform.localScale;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            fillTransform.localScale = Vector3.Lerp(initialScale, Vector3.one, t);
            yield return null;
        }

        fillTransform.localScale = Vector3.one;
    }

    public void SetRadius(float radius)
    {
        gameObject.transform.localScale = new Vector3(radius, radius, radius);
    }
    

}
