using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Movement speed
    [SerializeField] private Vector3 gravity = new(0, -.098f, 0); // Gravity force

    [SerializeField] private Transform groundCheck; // For checking if player is grounded
    [SerializeField] private LayerMask groundMask; // Ground layer mask

    private Vector3 velocity;
    private bool isGrounded;

    private Rigidbody rb;
    [SerializeField] private CollideSlide collideSlide;

    float horizontal = 0;
    float vertical = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable built-in gravity since we're applying custom gravity
    }

    private void Update()
    {
        // Get movement input (WASD/Arrow keys)
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        // Calculate movement direction
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        Vector3 moveAmount = moveDirection * moveSpeed;
        moveAmount.y = 0;

        // Handle player movement input
        transform.position += Move(moveAmount);
    }

    private Vector3 Move(Vector3 moveAmount)
    {
        moveAmount = collideSlide.CollideAndSlide(moveAmount, transform.position, 0, false, moveAmount);
        moveAmount += collideSlide.CollideAndSlide(gravity, transform.position + moveAmount, 0, true, gravity);

        return moveAmount;
    }
}
