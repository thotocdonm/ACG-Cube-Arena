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

    public void ShowTooltip(string title, string description, int currentLevelValue, int nextLevelValue, StatModifierType modifierType)
    {
        tooltipContainerUI.SetData(title, description, currentLevelValue, nextLevelValue, modifierType);
        tooltipContainerUI.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipContainerUI.gameObject.SetActive(false);
    }
}
