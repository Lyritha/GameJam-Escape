using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputLayer inputLayer;

    [SerializeField] private float moveSpeed = 2f; // Movement speed
    [SerializeField] private float runSpeed = 4f; // Movement speed

    [SerializeField] private float gravity = -9.8f;  // Gravitational acceleration


    private Rigidbody rb;
    [SerializeField] private CollideSlide collideSlide;
    [SerializeField] private GroundChecker ground;

    // Player state flags
    public Vector2 Movement { get; private set; } = Vector2.zero;
    public bool IsJumping { get; private set; } = false;
    public bool IsSprinting { get; private set; } = false;
    public bool IsFreeLooking { get; private set; } = false;

    private void OnEnable()
    {
        inputLayer.moveEvent += OnMove;
        inputLayer.jumpEvent += OnJump;
        inputLayer.sprintEvent += OnSprint;
        inputLayer.freeLookEvent += OnFreeLookAround;
    }

    private void OnDisable()
    {
        inputLayer.moveEvent -= OnMove;
        inputLayer.jumpEvent -= OnJump;
        inputLayer.sprintEvent -= OnSprint;
        inputLayer.freeLookEvent -= OnFreeLookAround;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable built-in gravity since we're applying custom gravity
    }

    private void FixedUpdate()
    {
        // Calculate movement direction
        Vector3 moveDirection = transform.right * Movement.x + transform.forward * Movement.y;
        Vector3 moveAmount = moveDirection * ( (IsSprinting ? runSpeed : moveSpeed));
        moveAmount.y = 0;

        // Handle player movement input
        transform.position += Move(moveAmount);
    }

    private Vector3 Move(Vector3 moveAmount)
    {
        moveAmount = collideSlide.CollideAndSlide(moveAmount, transform.position, 0, false, moveAmount);
        moveAmount += collideSlide.CollideAndSlide(Vector3.up * gravity, transform.position + moveAmount, 0, true, Vector3.up * gravity);

        return moveAmount;
    }

    private void OnMove(Vector2 input) => Movement = input;
    private void OnJump(bool input) => IsJumping = input;
    private void OnSprint(bool input) => IsSprinting = input;
    private void OnFreeLookAround(bool input) => IsFreeLooking = input;
}
