using UnityEngine;

[CreateAssetMenu(fileName = "WalkState", menuName = "Scriptable Objects/PlayerStates/WalkState")]
public class PlayerWalkState : PlayerBaseState
{
    [SerializeField] private float walkSpeed = 3f;

    public override void OnEnable() 
    { 
    }
    public override void OnDisable() { }

    public override void EnterState(PlayerController player) { }

    public override void UpdateState(PlayerController player)
    {
        Vector2 movement = player.Movement;
        Vector3 force = new Vector3(movement.x, 0f, movement.y) * (Time.deltaTime * 100);
        player.rb.AddRelativeForce(force * walkSpeed);

        if (player.IsJumping) player.SwitchState(player.JumpState);
        if (player.SlopeState.IsOnSlope(player)) player.SwitchState(player.SlopeState);
        if (player.IsSprinting) player.SwitchState(player.RunState);
    }

    public override void OnCollisionEnterState(PlayerController player, Collision collision) { }
}
