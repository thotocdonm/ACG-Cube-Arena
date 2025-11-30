using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;

    [Header("Upgradeable Stats")]
    [SerializeField] private List<StatUpgradeDataSO> upgradeableStats;

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

    void OnEnable()
    {
        SaveLoadManager.onDataLoaded += OnDataLoadedCallback;
    }
    void OnDisable()
    {
        SaveLoadManager.onDataLoaded -= OnDataLoadedCallback;
    }
    
    private void OnDataLoadedCallback(SaveData data)
    {
        Dictionary<StatType, int> unlockedStats = new Dictionary<StatType, int>();
        for (int i = 0; i < data.unlockedStats.Count; i++)
        {
            unlockedStats.Add(data.unlockedStats[i], data.unlockedSkillLevel[i]);
        }
        LoadSkillLevels(unlockedStats);
    }

    public int GetSkillLevel(StatType statType)
    {
        if (skillLevels.TryGetValue(statType, out int level))
        {
            return level;
        }
        return 0;
    }

    public bool CanUpgradeSkill(StatUpgradeDataSO statUpgradeData)
    {
        float cost = GetSkillUpgradeCost(statUpgradeData);
        bool isEnoughDiamonds = CurrencyManager.instance.IsEnoughDiamonds((int)cost);

        return isEnoughDiamonds && GetSkillLevel(statUpgradeData.statType) < statUpgradeData.maxLevel;
    }

    public float GetSkillUpgradeCost(StatUpgradeDataSO statUpgradeData)
    {
        return statUpgradeData.baseCost + GetSkillLevel(statUpgradeData.statType) * statUpgradeData.costIncreasePerLevel;
    }


    public void UpgradeSkill(StatUpgradeDataSO statUpgradeData)
    {
        if (!CanUpgradeSkill(statUpgradeData)) return;

        float cost = GetSkillUpgradeCost(statUpgradeData);
        CurrencyManager.instance.RemoveDiamonds((int)cost);

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

    public void InitializeSkillLevels()
    {
        skillLevels.Clear();
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            skillLevels.Add(statType, 0);
        }
    }

    public Dictionary<StatType, int> GetSkillLevelsDictionary()
    {
        return skillLevels;
    }

    public void LoadSkillLevels(Dictionary<StatType, int> unlockedStats)
    {
        skillLevels = unlockedStats;
        RecaculateAllUpgradeableStats();

    }
    
    private void RecaculateAllUpgradeableStats()
    {
        foreach (StatUpgradeDataSO statUpgradeData in upgradeableStats)
        {
            ApplySkillUpgrade(statUpgradeData);
        }
    }
}
