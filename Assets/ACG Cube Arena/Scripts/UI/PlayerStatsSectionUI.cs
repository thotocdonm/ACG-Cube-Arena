using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerStatsSectionUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform statContainerParent;

    [Header("UI")]
    [SerializeField] private RectTransform playerStatsSection;
    [SerializeField] private float shownXPos = -300;
    [SerializeField] private float hiddenXPos = 275;
    [SerializeField] private float slideDuration = 0.3f;

    private PlayerStats playerStats;
    private bool isShown = false;
    private StatType[] statToDisplay;

    void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        
    }
    
    void OnDestroy()
    {
        foreach (StatType statType in statToDisplay)
        {
            Stat stat = playerStats.GetStat(statType);
            if (stat != null)
            {
                stat.OnValueChanged -= OnStatValueChangedCallback;
            }
        }
    }

    void Start()
    {
        statToDisplay = new StatType[] {
            StatType.MaxHealth,
             StatType.MoveSpeed,
              StatType.AttackDamage,
                StatType.SkillCooldownReduction,
                 StatType.CriticalChance,
                  StatType.CriticalDamage,
                     StatType.DashCooldownReduction };

        foreach (StatType statType in statToDisplay)
        {
            Stat stat = playerStats.GetStat(statType);
            if (stat != null)
            {
                stat.OnValueChanged += OnStatValueChangedCallback;
            }
        }

        InitializeStatContainers();
    }


    [NaughtyAttributes.Button]
    private void InitializeStatContainers()
    {
        StatContainerUI[] statContainers = statContainerParent.GetComponentsInChildren<StatContainerUI>();

        for (int i = 0; i < statContainers.Length; i++)
        {
            StatType statType = statToDisplay[i];
            Stat stat = playerStats.GetStat(statType);
            bool isPercentage = statType == StatType.SkillCooldownReduction || statType == StatType.DashCooldownReduction;
            statContainers[i].Configure(ResourcesManager.instance.GetStatIcon(statType), Helper.SplitCamelCase(statType.ToString()), stat.GetValue(), isPercentage);
        }
    }

    public void TogglePlayerStatsSection()
    {
        isShown = !isShown;

        float targetX = isShown ? shownXPos : hiddenXPos;
        playerStatsSection.DOAnchorPosX(targetX, slideDuration).SetEase(Ease.OutCubic);
    }

    private void OnStatValueChangedCallback(float newStatValue)
    {
        InitializeStatContainers();
    }
    

    

}
