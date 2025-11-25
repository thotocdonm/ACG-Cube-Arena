using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stat", menuName = "Player/Player Stats", order = 0)]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Base Stats")]
    public string playerName;
    public float maxHealth;
    public float moveSpeed;

    [Header("Attack Stats")]
    public float attackDamage;
    public float criticalChance;
    public float criticalDamage;
    public float skillCooldownReduction;

    [Header("Dash Stats")]
    public float dashDistance;
    public float dashDuration;
    public float dashCooldownReduction;

    [Header("Ranged Specific")]
    public GameObject projectilePrefab;
    public float projectileSpeed;
}
