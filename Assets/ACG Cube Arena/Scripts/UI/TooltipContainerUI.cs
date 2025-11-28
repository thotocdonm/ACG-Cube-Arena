using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TooltipContainerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image priceIcon;

    [Header("Offset Settings")]
    [SerializeField] private Vector2 offsetRight = new Vector2(200f, 10f);
    [SerializeField] private Vector2 offsetLeft = new Vector2(-200f, 10f);

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPoint
        );
        Vector2 offset = localPoint.x < 0 ? offsetRight : offsetLeft;
        transform.position = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, 0f);

    }

    public void SetData(string title, string description, int currentLevelValue, int nextLevelValue, int priceValue, StatModifierType modifierType, bool isMaxLevel, bool isPercentage)
    {
        titleText.text = title;
        descriptionText.text = description;
        currentLevelText.text = $"Current Level: <color=green>+ {currentLevelValue} {(modifierType == StatModifierType.Percentage || isPercentage ? "%" : "")}</color>";
        nextLevelText.text = isMaxLevel ? "<color=green>MAXED</color>" : $"Next Level: <color=green>+ {nextLevelValue} {(modifierType == StatModifierType.Percentage || isPercentage ? "%" : "")}</color>";
        priceText.text = priceValue.ToString();
        if (isMaxLevel)
        {
            priceIcon.gameObject.SetActive(false);
            priceText.gameObject.SetActive(false);
        }
        else
        {
            priceIcon.gameObject.SetActive(true);
            priceText.gameObject.SetActive(true);
        }
    }
}
