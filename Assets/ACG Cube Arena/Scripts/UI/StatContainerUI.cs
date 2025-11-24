using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatContainerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image statIcon;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    public void Configure(Sprite statIcon, string statName, float statValue, bool isPercentage = false)
    {
        this.statIcon.sprite = statIcon;
        this.statNameText.text = statName;
        this.statValueText.text = isPercentage ? statValue.ToString("F0") + "%" : statValue.ToString("F1");

    }
}
