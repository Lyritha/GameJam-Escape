using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private InputLayer inputLayer;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] public Rigidbody Rb { get; private set; }

    //state machine
    PlayerBaseState currentState;
    public PlayerWalkState WalkState = new();
    public PlayerRunState RunState = new();
    public PlayerJumpState JumpState = new();

    public Vector2 movement { get; private set; } = Vector2.zero;

    public bool IsJumping { get; private set; } = false;

    private void OnEnable()
    {
        inputLayer.moveEvent += OnMove;
        inputLayer.jumpEvent += OnJump;
    }

    private void OnDisable()
    {
        inputLayer.moveEvent -= OnMove;
        inputLayer.jumpEvent -= OnJump;
    }

    private void Start()
    {
        SwitchState(WalkState);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void FixedUpdate()
    {
        if (false)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f; // Ignore any vertical rotation, we only care about horizontal movement
            cameraForward.Normalize(); // Ensure it's a unit vector

            // Get the camera's right direction
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0f; // Ignore any vertical rotation
            cameraRight.Normalize(); // Ensure it's a unit vector

            // Calculate the direction based on the camera's forward and right vectors
            Vector3 moveDirection = cameraForward * movement.y + cameraRight * movement.x;

            // Apply force relative to the camera's direction
            Vector3 force = moveDirection;
            Rb.AddForce(force);

            // Calculate the target rotation based on the camera's forward direction
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

            float rotationSpeed = 500f; // You can adjust this value to control the speed of rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnMove(Vector2 input) => movement = input;
    private void OnJump(bool input) => IsJumping = input;
}
