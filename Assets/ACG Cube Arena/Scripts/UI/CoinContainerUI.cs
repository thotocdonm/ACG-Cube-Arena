using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinContainerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI coinText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinText(CurrencyManager.instance.GetCurrentCoins());
        CurrencyManager.onCoinsChanged += OnCoinsChangedCallback;
    }

    void OnDestroy()
    {
        CurrencyManager.onCoinsChanged -= OnCoinsChangedCallback;
    }

    private void UpdateCoinText(int coins)
    {
        coinText.text = coins.ToString();
    }
    
    private void OnCoinsChangedCallback(int coins)
    {
        UpdateCoinText(coins);
    }
}
