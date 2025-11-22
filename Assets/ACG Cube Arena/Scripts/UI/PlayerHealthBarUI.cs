using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    private PlayerStats playerStats;


    void Awake()
    {
        
        
    }

    void OnDestroy()
    {
        playerStats.MaxHealth.OnValueChanged -= OnMaxHealthChangedCallback;
        PlayerStats.onPlayerHitted -= OnPlayerHittedCallback;
        
    }
    private void OnMaxHealthChangedCallback(float newMaxHealth)
    {
        Debug.Log("Max health changed: " + newMaxHealth);
        UpdateHealth((int)playerStats.CurrentHealth, (int)newMaxHealth);
    }


    public void Initialize(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
        PlayerStats.onPlayerHitted += OnPlayerHittedCallback;
        playerStats.MaxHealth.OnValueChanged += OnMaxHealthChangedCallback;
        UpdateHealth((int)playerStats.MaxHealth.GetValue(), (int)playerStats.MaxHealth.GetValue());
    }


    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        float fillAmount = (float)currentHealth / maxHealth;
        fillImage.fillAmount = fillAmount;
        playerHealthText.text = $"{currentHealth} / {maxHealth}";
    }
    
    private void OnPlayerHittedCallback(int currentHealth, int damage)
    {
        UpdateHealth(currentHealth, (int)playerStats.MaxHealth.GetValue());
    }
}
