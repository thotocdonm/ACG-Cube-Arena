using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using DG.Tweening;

public class MeleeEnemy : Enemy
{
    [Header("Charge Attack Settings")]
    [SerializeField] private LineRenderer chargeIndicator;
    [SerializeField] private float telegraphDuration;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float recoveryDuration;


    protected override void Awake()
    {
        base.Awake();
        AttackStrategy = new ChargeAttackStrategy(this, rb, playerTarget, stats, chargeIndicator, telegraphDuration, chargeSpeed, chargeDuration, recoveryDuration);
        
    }

}
