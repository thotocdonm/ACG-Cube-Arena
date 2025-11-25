using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class IchigoComboAttack : MonoBehaviour
{

    [Header("Animations")]
    [SerializeField] Animator animator;
    [SerializeField] string[] attackTrigger = { "Attack1", "Attack2", "Attack3" };

    [Header("Timing")]
    [SerializeField] private float attackCooldown = 0.25f;
    [SerializeField] private float comboWindow = 1.3f;

    [Header("Hitbox")]
    [SerializeField] private BoxCollider hitbox;

    [Header("Elements")]
    private PlayerController owner;

    [Header("Force")]
    [SerializeField] private float[] lungeDistance = { 1.5f, 1.2f, 4f };
    [SerializeField] private float[] lungeDuration = { 0.15f, 0.18f, 0.4f };

    private int currentComboIndex;
    private float nextAttackReadyTime;
    private float comboExpireTime;
    private bool isAttacking;
    private bool buffered;

    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        owner = GetComponent<PlayerController>();
    }

    private void Update()
    {

    }


    public bool TryStartAttack()
    {

        if(owner.StateMachine.CurrentState is DashingState) return false;

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
        if(!buffered && Time.time <= comboExpireTime)
        {
            buffered = true;
        }
    }


    private void PlayAttack(int index)
    {
        animator.SetTrigger(attackTrigger[index]);
    }

    //Animation Events
    public void EnableHitbox()
    {
        hitbox.enabled = true;
        StartCoroutine(LungeRoutice(currentComboIndex));
        
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    private IEnumerator LungeRoutice(int comboIndex)
    {
        float duration = lungeDuration[comboIndex];
        float distance = lungeDistance[comboIndex];
        float elapsed = 0;

        Vector3 start = transform.position;
        Vector3 dir =  transform.forward;

        Vector3 end = start + dir * distance;

        while (elapsed < duration)
        {
            yield return new WaitForFixedUpdate();
            elapsed += Time.fixedDeltaTime;

            float t = elapsed / duration;
            float easedT = 1f - Mathf.Pow(1f - t, 3f);
            Vector3 newPosition = Vector3.LerpUnclamped(start, end, easedT);
            owner.Rigidbody.MovePosition(newPosition);
            
        }
    }

    public void OnAttackAnimatonEnd()
    {
        Debug.Log("Play Attack Animation End");
        isAttacking = false;
        bool shouldContinueCombo = buffered || owner.IsAttackHeld;
        buffered = false;

        if (shouldContinueCombo && Time.time <= comboExpireTime && currentComboIndex < attackTrigger.Length - 1)
        {
            TryStartAttack();
            return;
        }
        
        if(Time.time > comboExpireTime)
        {
            currentComboIndex = 0;
        }
    }

    public void CancelCurrentAttack()
    {
        Debug.LogWarning("Cancelling Current Attack");
        isAttacking = false;
        buffered = false;
        currentComboIndex = 0;
        comboExpireTime = 0;
        StopAllCoroutines();
        
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        
        animator.Play("Idle");
        
        DisableHitbox();
    }
    
    public bool IsComboExpired()
    {
        return Time.time > comboExpireTime;
    }
}
