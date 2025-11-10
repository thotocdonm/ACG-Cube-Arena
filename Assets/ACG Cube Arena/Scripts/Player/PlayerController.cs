using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody rb;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector2 lastMoveInput;

    [Header("Dash")]
    private float lastDashTime = 100;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashDrag;

    [Header("State")]
    private StateMachine stateMachine;

    public IdleState idleState { get; private set; }
    public MovingState movingState { get; private set; }
    public DashingState dashingState { get; private set; }

    public Rigidbody Rigidbody => rb;
    public Animator Animator => animator;
    public Vector2 LastMoveInput => lastMoveInput;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float DashSpeed => dashSpeed;
    public float DashDuration => dashDuration;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();

        idleState = new IdleState(this, stateMachine);
        movingState = new MovingState(this, stateMachine);
        dashingState = new DashingState(this, stateMachine);

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

    // Update is called once per frame
    void Update()
    {
        //increment the last dash time
        lastDashTime += Time.deltaTime;
        stateMachine?.Update();
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
        return lastDashTime >= dashCooldown;
    }

    public void ResetDashCooldown()
    {
        lastDashTime = 0;
    }

    #if UNITY_EDITOR
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"State: {stateMachine.CurrentState.GetType().Name}");
    }
    #endif
}
