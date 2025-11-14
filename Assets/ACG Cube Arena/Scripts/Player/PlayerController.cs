using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    private PlayerStats playerStats;
    private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private IchigoComboAttack ichigoComboAttack;
    private bool isAttackHeld;
    private bool bufferedAttackFromDash;

    [Header("Mouse Aim")]
    [SerializeField] private LayerMask groundLayer;
    private Vector3 aimDirection;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed;
    private Vector2 lastMoveInput;

    private float lastDashTime = 100;

    [Header("State")]
    private StateMachine stateMachine;

    public IdleState idleState { get; private set; }
    public MovingState movingState { get; private set; }
    public DashingState dashingState { get; private set; }
    public IchigoAttackingState ichigoAttackingState { get; private set; }

    public Rigidbody Rigidbody => rb;
    public StateMachine StateMachine => stateMachine;
    public Animator Animator => animator;
    public Vector2 LastMoveInput => lastMoveInput;
    public float MoveSpeed => playerStats.MoveSpeed.GetValue();
    public float RotationSpeed => rotationSpeed;
    public float DashSpeed => playerStats.DashSpeed.GetValue();
    public float DashDuration => playerStats.DashDuration.GetValue();
    public float AttackDamage => playerStats.AttackDamage.GetValue();
    public float CriticalChance => playerStats.CriticalChance.GetValue();
    public float CriticalDamage => playerStats.CriticalDamage.GetValue();
    public Vector3 AimDirection => aimDirection;
    public IchigoComboAttack IchigoComboAttack => ichigoComboAttack;
    public bool IsAttackHeld => isAttackHeld;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
        stateMachine = new StateMachine();

        idleState = new IdleState(this, stateMachine);
        movingState = new MovingState(this, stateMachine);
        dashingState = new DashingState(this, stateMachine);
        ichigoAttackingState = new IchigoAttackingState(this, stateMachine);

        stateMachine.Initialize(idleState);
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Move(InputAction.CallbackContext context)
    {
        lastMoveInput = context.ReadValue<Vector2>();
        var currentState = stateMachine.CurrentState as PlayerBaseState;
        currentState?.HandleMove(lastMoveInput);
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var currentState = stateMachine.CurrentState as PlayerBaseState;
            currentState?.HandleDash();
        }

    }
    
    public void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack");


        if (context.performed)
        {
            var currentState = stateMachine.CurrentState as PlayerBaseState;
            currentState?.HandleAttack();
        }
        
        if(context.started)
        {
            isAttackHeld = true;
        }
        else if(context.canceled)
        {
            isAttackHeld = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //increment the last dash time
        lastDashTime += Time.deltaTime;
        stateMachine?.Update();
        UpdateAimDirection();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
    }

    public void ChangeState(PlayerBaseState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public bool CanDash()
    {
        return lastDashTime >= playerStats.DashCooldown.GetValue();
    }

    public void ResetDashCooldown()
    {
        lastDashTime = 0;
    }

    private void UpdateAimDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            direction.y = 0;
            if(direction.sqrMagnitude > 0.01f)
            {
                aimDirection = direction;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player collided with " + collision.gameObject.name);
    }

    #if UNITY_EDITOR
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"State: {stateMachine.CurrentState.GetType().Name}");
    }
    #endif
}
