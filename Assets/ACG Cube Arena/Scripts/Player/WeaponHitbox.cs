using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private PlayerController owner;

    [Header("VFX")]
    [SerializeField] private GameObject hitVFX;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //Deal damage
            Debug.Log("Hit Enemy and deal damage" + owner.AttackDamage);
            other.gameObject.GetComponent<EnemyStats>().TakeDamage((int)owner.AttackDamage);
            
            //Vfx when hitting enemy
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Quaternion rotation = Quaternion.Euler(0,Random.Range(0,360),0);
            GameObject vfxInstance =Instantiate(hitVFX, contactPoint, rotation);
            Destroy(vfxInstance, 1f);

            //Shake camera
            CameraShakeManager.instance.ShakeCamera(1, 0.1f);

            //Hit stop when hitting enemy
            HitStopManager.instance.DoHitStop(0.05f, 0);
        }
    }
}
