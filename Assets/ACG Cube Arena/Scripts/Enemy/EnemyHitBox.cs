using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private EnemyStats enemyStats;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Enemy hitbox triggered");
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStats>().TakeDamage((int)enemyStats.AttackDamage.GetValue());
        }
    }
}
