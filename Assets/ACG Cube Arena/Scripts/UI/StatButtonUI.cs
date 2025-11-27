using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Elements")]
    [SerializeField] private GameObject background;

    [Header("Stat Upgrade Data")]
    [SerializeField] private StatUpgradeDataSO statUpgradeData;

    


    void Awake()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(background != null)
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
        Debug.Log("OnPointerExit");
        TooltipUIManager.instance.HideTooltip();
    }

    void OnDisable()
    {
        if (background != null)
        {
            background.SetActive(false);
        }
    }

    private void PrepareAndShowTooltip()
    {
        string title = statUpgradeData.statTooltip.title;
        string description = statUpgradeData.statTooltip.description;
        StatModifierType modifierType = statUpgradeData.modifierType;
        int currentLevelValue = 10;
        int nextLevelValue = 20;
        TooltipUIManager.instance.ShowTooltip(title, description, currentLevelValue, nextLevelValue, modifierType);
    }
}
