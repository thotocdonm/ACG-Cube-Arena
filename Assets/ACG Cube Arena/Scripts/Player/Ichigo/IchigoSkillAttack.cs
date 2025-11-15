using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class IchigoSkillAttack : MonoBehaviour
{
    [Header("Skill Settings")]
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float minChargeTime = 0.5f;
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float baseMultiplier = 1f;
    [SerializeField] private float maxChargeMultiplier = 2f;

    private PlayerController owner;
    private float chargeStartTime;
    private bool isCharging = false;

    public bool IsCharging => isCharging;

    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartCharging()
    {
        if (isCharging) return;
        Debug.Log("Start Charging");
        owner.Animator.SetTrigger("StartCharging");
        isCharging = true;
        chargeStartTime = Time.time;
    }

    public void ReleaseSkill()
    {
        if (!isCharging) return;
        Debug.Log("Release Skill");

        isCharging = false;
        float chargeDuration = Time.time - chargeStartTime;
        Debug.Log("Charge Duration: " + chargeDuration);

        if (chargeDuration < minChargeTime)
        {
            Debug.Log("Charge Duration is too short");
            owner.Animator.ResetTrigger("StartCharging");
            owner.Animator.SetTrigger("SkillCancel");
            return;
        }

        FireSkill(chargeDuration);
    }

    public void FireSkill(float chargeDuration)
    {
        Debug.Log("Fire Skill");
        owner.Animator.ResetTrigger("StartCharging");
        owner.Animator.SetTrigger("SkillRelease");
        float multiplier = Mathf.Lerp(baseMultiplier, maxChargeMultiplier, chargeDuration / maxChargeTime);

        GameObject skillInstance = Instantiate(skillPrefab, firePoint.position, firePoint.rotation);
        if(skillInstance.GetComponentInChildren<IchigoSkillProjectile>())
        {
            IchigoSkillProjectile projectile = skillInstance.GetComponentInChildren<IchigoSkillProjectile>();
            float damage = owner.GetCriticalDamage();
            bool isCritical = damage > owner.AttackDamage;
            float finalDamage = damage * multiplier;
            Debug.Log("Final Damage: " + finalDamage);
            projectile.Initialize((int)finalDamage, isCritical);
        }

    }
    
    public void CancelSkill()
    {
        if (!isCharging) return;
        owner.Animator.ResetTrigger("StartCharging");
        owner.Animator.SetTrigger("SkillCancel");
        isCharging = false;
    }


}
