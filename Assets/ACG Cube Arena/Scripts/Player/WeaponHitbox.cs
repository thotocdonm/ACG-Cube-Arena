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
            bool isCritical = Random.Range(0f, 100f) < owner.CriticalChance;
            int damage = 0;
            if (isCritical)
            {
                damage = (int)(owner.AttackDamage * owner.CriticalDamage / 100);
            }
            else
            {
                damage = (int)owner.AttackDamage;
            }

            Vector3 contactPoint = other.ClosestPoint(transform.position);
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(damage, isCritical, contactPoint);
        }
    }
}
