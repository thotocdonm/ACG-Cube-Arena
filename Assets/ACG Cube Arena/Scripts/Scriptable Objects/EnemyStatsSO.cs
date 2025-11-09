using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy/Enemy Stats",order = 0)]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Base Stats")]
    public string enemyName;
    public float maxHealth;
    public float moveSpeed;

    [Header("Attack Stats")]
    public float attackDamage;
    public float attackRange;
    public float attackCooldown;
    public float detectionRange;

    [Header("Ranged Specific")]
    public GameObject projectilePrefab;
    public float projectileSpeed;

}
