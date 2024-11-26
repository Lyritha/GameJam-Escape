using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private InputLayer inputLayer;

    public Rigidbody rb;
    [SerializeField] private Collider playerCollider;

    //state machine
    private PlayerBaseState currentMovementState;
    [Header("StateMachine objects")]
    public PlayerWalkState WalkState;
    public PlayerRunState RunState;
    public PlayerJumpState JumpState;
    public PlayerWalkSlopeState SlopeState;

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
        currentMovementState = state;
        state.EnterState(this);
    }

    private void FixedUpdate()
    {
        if (!IsFreeLooking)
        {
            // Get camera directions, ignoring vertical rotation
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // Rotate towards camera's forward direction
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f * Time.deltaTime);
        }
    }

    private void OnMove(Vector2 input) => Movement = input;
    private void OnJump(bool input) => IsJumping = input;
    private void OnSprint(bool input) => IsSprinting = input;
    private void OnFreeLookAround(bool input) => IsFreeLooking = input;
}
