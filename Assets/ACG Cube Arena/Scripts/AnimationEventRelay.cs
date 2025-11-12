using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    [SerializeField] private IchigoComboAttack combo;

    public void EnableHitbox() => combo.EnableHitbox();
    public void DisableHitbox() => combo.DisableHitbox();
    public void OnAttackAnimationEnd() => combo.OnAttackAnimatonEnd();
}
