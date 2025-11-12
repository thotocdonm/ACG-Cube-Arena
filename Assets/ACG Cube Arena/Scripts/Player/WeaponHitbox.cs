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
            Debug.Log("Hit Enemy and deal damage" + owner.AttackDamage);
            other.gameObject.GetComponent<EnemyStats>().TakeDamage((int)owner.AttackDamage);
        }
    }
}
