using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private InputLayer inputLayer;
    [SerializeField] private Rigidbody rb;

    public CollideSlide collideSlide;
    public Vector3 velocity = Vector3.zero;

    // State machine
    private PlayerBaseState currentMovementState;
    [Header("StateMachine objects")]
    public PlayerBaseState WalkState;
    public PlayerBaseState RunState;
    public PlayerBaseState JumpState;
    public PlayerBaseState fallState;

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

    private void Start() => SwitchState(WalkState);

    private void Update() => currentMovementState.UpdateState(this);

    private void OnCollisionEnter(Collision collision) => currentMovementState.OnCollisionEnterState(this, collision);

    public void SwitchState(PlayerBaseState state)
    {
        Move();

        currentMovementState = state;
        state.EnterState(this);
    }

    private void FixedUpdate()
    {

        UpdateRotation();
    }


    private void UpdateRotation()
    {
        // Rotate towards the camera direction (non-physics-related, in Update)
        if (!IsFreeLooking)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5);
        }
    }

    public void Move()
    {
        // Apply velocity adjustments for collisions and gravity
        //velocity += Physics.gravity * Time.fixedDeltaTime;
        velocity = collideSlide.CollideAndSlide(velocity, transform.position, 0, false, velocity);
        //velocity += collideSlide.CollideAndSlide(Physics.gravity, transform.position + velocity, 0, true, Physics.gravity);

        // Apply the movement to the player's position
        transform.localPosition += velocity;

        // Apply damping
        velocity *= 0.9f; // Adjust as needed
    }

    private void OnMove(Vector2 input) => Movement = input;
    private void OnJump(bool input) => IsJumping = input;
    private void OnSprint(bool input) => IsSprinting = input;
    private void OnFreeLookAround(bool input) => IsFreeLooking = input;
}
