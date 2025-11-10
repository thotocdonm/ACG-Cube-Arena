using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private RectTransform healthBar;

    [Header("Settings")]
    private float health;
    private float maxHealth;
    [SerializeField] private float width;
    [SerializeField] private float height;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }
    
    public void SetHealth(float health)
    {
        this.health = health;
        float newWidth = width * (health / maxHealth);

        healthBar.sizeDelta = new Vector2(newWidth, height);
    }
}
