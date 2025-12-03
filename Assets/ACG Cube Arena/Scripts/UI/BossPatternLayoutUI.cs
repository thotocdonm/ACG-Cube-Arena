using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternLayoutUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform iconContainer;
    [SerializeField] private BossPatternIconUI iconPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        GameEventsManager.onFinalBossPatternStarted += OnFinalBossPatternStartedCallback;
    }

    void OnDestroy()
    {
        GameEventsManager.onFinalBossPatternStarted -= OnFinalBossPatternStartedCallback;
    }

    private void OnFinalBossPatternStartedCallback(FinalBossPatternIconSO iconSO, float duration)
    {
        BossPatternIconUI icon = Instantiate(iconPrefab, iconContainer);
        icon.Configure(iconSO, duration);
    }
}
