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
    private PlayerBaseState currentState;

    public IdleState idleState { get; private set; }
    public MovingState movingState { get; private set; }
    public DashingState dashingState { get; private set; }

    public Rigidbody Rigidbody => rb;
    public Vector2 LastMoveInput => lastMoveInput;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float DashSpeed => dashSpeed;
    public float DashDuration => dashDuration;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        idleState = new IdleState(this, rb);
        movingState = new MovingState(this, rb);
        dashingState = new DashingState(this, rb);

        currentState = idleState;
        currentState.Enter();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Move(InputAction.CallbackContext context)
    {
        lastMoveInput = context.ReadValue<Vector2>();
        Debug.Log(lastMoveInput);
        currentState?.HandleMove(lastMoveInput);
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentState?.HandleDash();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //increment the last dash time
        lastDashTime += Time.deltaTime;
        currentState?.Update();
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void ChangeState(PlayerBaseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
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
        GUI.Label(new Rect(10, 10, 200, 20), $"State: {currentState.GetType().Name}");
    }
    #endif
}
