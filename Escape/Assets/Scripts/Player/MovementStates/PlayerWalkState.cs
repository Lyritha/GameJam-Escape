using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WalkState", menuName = "Scriptable Objects/PlayerStates/WalkState")]
public class PlayerWalkState : PlayerBaseState
{
    [SerializeField] private float walkSpeed = 3f;

    public override void OnEnable() { }
    public override void OnDisable() { }

    public override void EnterState(PlayerController player) { }

    public override void UpdateState(PlayerController player)
    {
        // Get player input for movement
        Vector2 movement = player.Movement;

        // Calculate velocity based on input and walking speed
        player.velocity += Time.deltaTime * walkSpeed * new Vector3(movement.x, 0f, movement.y);

        // Check if the player should switch states
        if (player.IsJumping) player.SwitchState(player.JumpState);
        if (player.IsSprinting) player.SwitchState(player.RunState);
    }

    public override void OnCollisionEnterState(PlayerController player, Collision collision) { }
}