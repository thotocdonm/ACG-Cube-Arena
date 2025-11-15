using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private PlayerController owner;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //Deal damage
            int damage = owner.GetCriticalDamage();
            bool isCritical = damage > owner.AttackDamage;

            Vector3 contactPoint = other.ClosestPoint(transform.position);
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(damage, isCritical, contactPoint);
        }
    }
}
