using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 moveDirection;

    [Header("Dash")]
    private Vector3 dashDirection;
    private float lastDashTime = 100;
    private float dashTimer;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashDrag;

    [Header("State")]
    private PlayerState currentState = PlayerState.Normal;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(context.performed && currentState == PlayerState.Normal && lastDashTime >= dashCooldown)
        {
            Debug.Log("Prepare to dash");
            HandleDash();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //increment the last dash time
        lastDashTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        switch(currentState){
            case PlayerState.Normal:
                HandleMovement();
                HandleRotation();
                break;
            case PlayerState.Dashing:
                HandleDashingState();
                break;
        }
    }

    private void HandleMovement()
    {
        Vector3 newVelocity = new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed;
        rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
    }

    private void HandleRotation()
    {
        Vector3 lookDirection = new Vector3(moveDirection.x, 0, moveDirection.y); 
        if(lookDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }
    }

    private void HandleDash()
    {
        Debug.Log("Dash");
        lastDashTime = 0;
        currentState = PlayerState.Dashing;
        if (moveDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }
        else
        {
            dashDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
        }

        rb.velocity = dashDirection * dashSpeed;

    }
    
    private void HandleDashingState()
    {
        dashTimer += Time.deltaTime;
        if(dashTimer >= dashDuration){
            currentState = PlayerState.Normal;
            dashTimer = 0;
        }
    }
    
}
