using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private TextMeshProUGUI bossHealthText;
    private EnemyStats trackedBossStats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(EnemyStats bossStats)
    {
        trackedBossStats = bossStats;
        bossNameText.text = trackedBossStats.GetBaseStats().enemyName;
        EnemyStats.onBossHealthChanged += OnBossHealthChangedCallback;
        UpdateHealth((int)trackedBossStats.MaxHealth.GetValue(), (int)trackedBossStats.MaxHealth.GetValue());
    }


    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        float fillAmount = (float)currentHealth / maxHealth;
        fillImage.fillAmount = fillAmount;
        bossHealthText.text = $"{currentHealth} / {maxHealth}";
    }
    
    private void OnBossHealthChangedCallback(int currentHealth)
    {
        UpdateHealth(currentHealth, (int)trackedBossStats.MaxHealth.GetValue());
    }
}
