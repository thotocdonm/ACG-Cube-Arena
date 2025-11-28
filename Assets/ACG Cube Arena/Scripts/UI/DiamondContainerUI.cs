using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiamondContainerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI diamondText;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        UpdateDiamondText(CurrencyManager.instance.GetCurrentDiamonds());
        CurrencyManager.onDiamondsChanged += OnDiamondsChangedCallback;
    }

    void OnDestroy()
    {
        CurrencyManager.onDiamondsChanged -= OnDiamondsChangedCallback;
    }

    private void UpdateDiamondText(int diamonds)
    {
        diamondText.text = diamonds.ToString();
    }
    
    private void OnDiamondsChangedCallback(int diamonds)
    {
        UpdateDiamondText(diamonds);
    }
}
