using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPatternIconUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image iconImage;

    [Header("Settings")]
    [SerializeField] private float blinkSpeed;
    [SerializeField] private float blinkThreshold;

    private float duration;
    private float remaining;

    private Coroutine routine;
    
    private void OnDisable()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        routine = null;
    }

    public void Configure(FinalBossPatternIconSO iconSO, float duration)
    {
        this.duration = duration;
        remaining = duration;

        iconImage.sprite = iconSO.icon;

        if (routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(TimerRoutine());
    }
    
    private IEnumerator TimerRoutine()
    {
        while (remaining > 0)
        {
            remaining -= Time.deltaTime;
            if (remaining <= blinkThreshold)
            {
                float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
                Color color = iconImage.color;
                color.a = alpha;
                iconImage.color = color;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
