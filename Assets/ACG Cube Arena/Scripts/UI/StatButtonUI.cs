using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Elements")]
    [SerializeField] private GameObject background;
    [SerializeField] private Button button;
    [SerializeField] private Image statIcon;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Stat Upgrade Data")]
    [SerializeField] private StatUpgradeDataSO statUpgradeData;

    


    void Awake()
    {
        SkillTreeManager.onSkillLevelUpgraded += OnSkillLevelUpgradedCallback;
    }
    void OnDestroy()
    {
        SkillTreeManager.onSkillLevelUpgraded -= OnSkillLevelUpgradedCallback;
    }
    void Start()
    {
        Configure();
    }

    void Update()
    {
        button.interactable = SkillTreeManager.instance.CanUpgradeSkill(statUpgradeData, out float skillUpgradeCost);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(background != null && button.interactable)
        {
            background.SetActive(true);
        }
        PrepareAndShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (background != null)
        {
            background.SetActive(false);
        }
        TooltipUIManager.instance.HideTooltip();
    }

    void OnDisable()
    {
        if (background != null)
        {
            background.SetActive(false);
        }
    }

    public void Configure()
    {
        statIcon.sprite = ResourcesManager.instance.GetStatIcon(statUpgradeData.statType);
        levelText.text = $"{SkillTreeManager.instance.GetSkillLevel(statUpgradeData.statType)} / {statUpgradeData.maxLevel}";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnUpgradeClick);

    }

    private void PrepareAndShowTooltip()
    {
        float currentSkillUpgradeCost = SkillTreeManager.instance.CanUpgradeSkill(statUpgradeData, out float skillUpgradeCost) ? skillUpgradeCost : 0;
        string title = statUpgradeData.statTooltip.title;
        string description = statUpgradeData.statTooltip.description;
        StatModifierType modifierType = statUpgradeData.modifierType;
        int currentLevelValue = Mathf.RoundToInt(SkillTreeManager.instance.GetSkillLevel(statUpgradeData.statType) * statUpgradeData.valuePerLevel);
        bool isMaxLevel = SkillTreeManager.instance.GetSkillLevel(statUpgradeData.statType) == statUpgradeData.maxLevel;
        int nextLevelValue = Mathf.RoundToInt((SkillTreeManager.instance.GetSkillLevel(statUpgradeData.statType) + 1) * statUpgradeData.valuePerLevel);

        bool isPercentage = statUpgradeData.statType == StatType.SkillCooldownReduction ||
        statUpgradeData.statType == StatType.DashCooldownReduction ||
        statUpgradeData.statType == StatType.CriticalChance ||
        statUpgradeData.statType == StatType.CriticalDamage;
        
        TooltipUIManager.instance.ShowTooltip(title, description, currentLevelValue, nextLevelValue, (int)currentSkillUpgradeCost, modifierType, isMaxLevel, isPercentage);
    }

    private void OnUpgradeClick()
    {
        SkillTreeManager.instance.UpgradeSkill(statUpgradeData);
    }

    private void OnSkillLevelUpgradedCallback(StatType statType, int newLevel)
    {
        if (statType == statUpgradeData.statType)
        {
            PrepareAndShowTooltip();
            levelText.text = $"{newLevel} / {statUpgradeData.maxLevel}";
            button.interactable = SkillTreeManager.instance.CanUpgradeSkill(statUpgradeData, out float skillUpgradeCost);
        }
        
    }

    
}
