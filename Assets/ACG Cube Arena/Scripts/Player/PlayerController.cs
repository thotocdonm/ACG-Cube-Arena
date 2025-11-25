using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    private PlayerStats playerStats;
    private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private IchigoComboAttack ichigoComboAttack;
    [SerializeField] private IchigoSkillAttack ichigoSkillAttack;

    private bool isAttackHeld;

    [Header("Mouse Aim")]
    [SerializeField] private LayerMask groundLayer;
    private Vector3 aimDirection;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float dashCooldown = 5f;
    private Vector2 lastMoveInput;

    private float lastDashTime = 100;

    [Header("State")]
    private StateMachine stateMachine;

    public IdleState idleState { get; private set; }
    public MovingState movingState { get; private set; }
    public DashingState dashingState { get; private set; }
    public IchigoAttackingState ichigoAttackingState { get; private set; }
    public IchigoChargingState ichigoChargingState { get; private set; }

    public Rigidbody Rigidbody => rb;
    public StateMachine StateMachine => stateMachine;
    public Animator Animator => animator;
    public Vector2 LastMoveInput => lastMoveInput;
    public float MoveSpeed => playerStats.MoveSpeed.GetValue();
    public float RotationSpeed => rotationSpeed;
    public float DashDistance => playerStats.DashDistance.GetValue();
    public float DashDuration => playerStats.DashDuration.GetValue();
    public float AttackDamage => playerStats.AttackDamage.GetValue();
    public float CriticalChance => playerStats.CriticalChance.GetValue();
    public float CriticalDamage => playerStats.CriticalDamage.GetValue();
    public float SkillCooldownReduction => playerStats.SkillCooldownReduction.GetValue();
    public float DashCooldownReduction => playerStats.DashCooldownReduction.GetValue();
    public Vector3 AimDirection => aimDirection;
    public IchigoComboAttack IchigoComboAttack => ichigoComboAttack;
    public IchigoSkillAttack IchigoSkillAttack => ichigoSkillAttack;
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
        ichigoChargingState = new IchigoChargingState(this, stateMachine);

        stateMachine.Initialize(idleState);

        GameStateManager.onGameStateChanged += OnGameStateChangedCallback;
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    
    void OnDestroy()
    {
        GameStateManager.onGameStateChanged -= OnGameStateChangedCallback;
    }

    private void OnGameStateChangedCallback(GameState newGameState)
    {
        if(newGameState != GameState.Game)
        {
            ChangeState(idleState);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(GameStateManager.instance.CurrentGameState != GameState.Game) return;
        lastMoveInput = context.ReadValue<Vector2>();
        var currentState = stateMachine.CurrentState as PlayerBaseState;
        currentState?.HandleMove(lastMoveInput);
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(GameStateManager.instance.CurrentGameState != GameState.Game) return;
        if (context.performed)
        {
            var currentState = stateMachine.CurrentState as PlayerBaseState;
            currentState?.HandleDash();
        }

    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.LogWarning("Attack button is over UI");
        }
        if (context.started)
        {
            isAttackHeld = true;
        }
        else if (context.canceled)
        {
            isAttackHeld = false;
        }


        if (GameStateManager.instance.CurrentGameState != GameState.Game) return;
        if (context.performed)
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;
            var currentState = stateMachine.CurrentState as PlayerBaseState;
            currentState?.HandleAttack();
        }


    }
    
    public void Skill(InputAction.CallbackContext context)
    {
        if(GameStateManager.instance.CurrentGameState != GameState.Game) return;
        if (!ichigoSkillAttack.IsSkillReady()) return;
        
        if (context.started)
        {
            if(stateMachine.CurrentState is IdleState || stateMachine.CurrentState is MovingState)
            {
                ChangeState(ichigoChargingState);
            }
        } 
        else if (context.canceled)
        {
            if(stateMachine.CurrentState is IchigoChargingState)
            {
                ichigoSkillAttack.ReleaseSkill();

                if(lastMoveInput.magnitude > 0.1f)
                {
                    ChangeState(movingState);
                }
                else
                {
                    ChangeState(idleState);
                }
            }
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
        return lastDashTime >= GetDashCooldown();
    }

    public void ResetDashCooldown()
    {
        lastDashTime = 0;
        GameEventsManager.TriggerSkillCooldownStart(SkillId.PlayerDash, GetDashCooldown());
    }
    
    private float GetDashCooldown()
    {
        float cdrValue = DashCooldownReduction;
        float finalCooldown = dashCooldown * (1 - cdrValue/100);
        return finalCooldown;
    }

    private void UpdateAimDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            direction.y = 0;
            if (direction.sqrMagnitude > 0.01f)
            {
                aimDirection = direction;
            }
        }
    }

    public int GetCriticalDamage()
    {
        bool isCritical = Random.Range(0f, 100f) < CriticalChance;
        int damage = 0;
        if (isCritical)
        {
            damage = (int)(AttackDamage * CriticalDamage / 100);
        }
        else
        {
            damage = (int)AttackDamage;
        }
        return damage;
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    #if UNITY_EDITOR
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"State: {stateMachine.CurrentState.GetType().Name}");
    }
    #endif
}
