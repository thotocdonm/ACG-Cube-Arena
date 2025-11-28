using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipUIManager : MonoBehaviour
{
    public static TooltipUIManager instance;

    [Header("Elements")]
    [SerializeField] private TooltipContainerUI tooltipContainerUI;

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

    public void ShowTooltip(string title, string description, int currentLevelValue, int nextLevelValue, int priceValue, StatModifierType modifierType, bool isMaxLevel, bool isPercentage = false)
    {
        tooltipContainerUI.SetData(title, description, currentLevelValue, nextLevelValue, priceValue, modifierType, isMaxLevel, isPercentage);
        tooltipContainerUI.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipContainerUI.gameObject.SetActive(false);
    }
}
