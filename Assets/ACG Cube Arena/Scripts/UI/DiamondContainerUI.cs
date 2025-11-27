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
        UpdateDiamondText(CurrencyManager.instance.GetCurrentCoins());
        CurrencyManager.onCoinsChanged += OnDiamondsChangedCallback;
    }

    void OnDestroy()
    {
        CurrencyManager.onCoinsChanged -= OnDiamondsChangedCallback;
    }

    private void UpdateDiamondText(int coins)
    {
        diamondText.text = coins.ToString();
    }
    
    private void OnDiamondsChangedCallback(int diamonds)
    {
        UpdateDiamondText(diamonds);
    }
}
