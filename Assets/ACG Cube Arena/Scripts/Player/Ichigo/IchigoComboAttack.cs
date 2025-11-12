using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IchigoComboAttack : MonoBehaviour
{

    [Header("Animations")]
    [SerializeField] Animator animator;
    [SerializeField] string[] attackTrigger = { "Attack1", "Attack2", "Attack3" };

    [Header("Timing")]
    [SerializeField] private float attackCooldown = 0.25f;
    [SerializeField] private float comboWindow = 1f;
    private float timer;

    [Header("Hitbox")]
    [SerializeField] private BoxCollider hitbox;

    private int currentComboIndex;
    private float nextAttackReadyTime;
    private float comboExpireTime;
    private bool isAttacking;
    private bool buffered;

    public bool IsAttacking => isAttacking;

    private void Update()
    {
        timer += Time.deltaTime;
    }


    public bool TryStartAttack()
    {
        float now = Time.time;
        if (now < nextAttackReadyTime) return false;

        if (now <= comboExpireTime)
        {
            currentComboIndex = Mathf.Min(currentComboIndex + 1, attackTrigger.Length - 1);
        }
        else
        {
            currentComboIndex = 0;
        }

        PlayAttack(currentComboIndex);

        nextAttackReadyTime = now + attackCooldown;
        comboExpireTime = now + comboWindow;
        isAttacking = true;
        buffered = false;
        return true;
    }

    public void BufferNextAttack()
    {
        if(Time.time <= comboExpireTime)
        {
            buffered = true;
        }
    }


    private void PlayAttack(int index)
    {
        Debug.Log("Playing Attack: " + attackTrigger[index]);
        animator.SetTrigger(attackTrigger[index]);
    }

    //Animation Events
    public void EnableHitbox()
    {
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    public void OnAttackAnimatonEnd()
    {
        Debug.Log("Attack Animation End");
        isAttacking = false;

        if (buffered && Time.time <= comboExpireTime && currentComboIndex < attackTrigger.Length - 1)
        {
            TryStartAttack();
            return;
        }
        else
        {
            buffered = false;
            if (Time.time > comboExpireTime)
            {
                currentComboIndex = 0;
            }

        }
    }
    
    public bool IsComboExpired()
    {
        return Time.time > comboExpireTime;
    }
}
