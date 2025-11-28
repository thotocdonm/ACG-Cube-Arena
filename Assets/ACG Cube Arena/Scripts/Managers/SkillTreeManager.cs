using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;

    private Dictionary<StatType, int> skillLevels = new Dictionary<StatType, int>();

    private PlayerStats playerStats;

    public static Action<StatType, int> onSkillLevelUpgraded;

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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeSkillLevels();
    }


    public int GetSkillLevel(StatType statType)
    {
        if (skillLevels.TryGetValue(statType, out int level))
        {
            return level;
        }
        return 0;
    }

    public bool CanUpgradeSkill(StatUpgradeDataSO statUpgradeData, out float skillUpgradeCost)
    {
        float cost = statUpgradeData.baseCost + GetSkillLevel(statUpgradeData.statType) * statUpgradeData.costIncreasePerLevel;
        bool isEnoughDiamonds = CurrencyManager.instance.IsEnoughDiamonds((int)cost);

        skillUpgradeCost = cost;
        return isEnoughDiamonds && GetSkillLevel(statUpgradeData.statType) < statUpgradeData.maxLevel;
    }


    public void UpgradeSkill(StatUpgradeDataSO statUpgradeData)
    {
        if (!CanUpgradeSkill(statUpgradeData, out float skillUpgradeCost)) return;

        CurrencyManager.instance.RemoveDiamonds((int)skillUpgradeCost);

        int currentLevel = GetSkillLevel(statUpgradeData.statType);
        skillLevels[statUpgradeData.statType] = currentLevel + 1;

        ApplySkillUpgrade(statUpgradeData);

        onSkillLevelUpgraded?.Invoke(statUpgradeData.statType, currentLevel + 1);

    }

    private void ApplySkillUpgrade(StatUpgradeDataSO statUpgradeData)
    {
        if (playerStats == null) return;

        Stat targetStat = playerStats.GetStat(statUpgradeData.statType);

        int currentLevel = GetSkillLevel(statUpgradeData.statType);
        float value = statUpgradeData.valuePerLevel * currentLevel;
        float newBaseValue = targetStat.GetOriginalBaseValue() + value;
        
        targetStat.SetBaseValue(newBaseValue);
    }
    
    private void InitializeSkillLevels()
    {
        skillLevels.Clear();
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            skillLevels.Add(statType, 0);
        }
    }
}
