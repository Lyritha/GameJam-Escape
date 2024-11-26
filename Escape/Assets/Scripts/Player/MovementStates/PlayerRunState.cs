using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RunState", menuName = "Scriptable Objects/PlayerStates/RunState")]
public class PlayerRunState : PlayerBaseState
{
    [SerializeField] private float runSpeed = 5;

    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }

    public override void EnterState(PlayerController player)
    {
    }


    public override void UpdateState(PlayerController player)
    {
        Vector2 movement = player.Movement;
        Vector3 force = new Vector3(movement.x, 0f, movement.y) * (Time.deltaTime * 100);
        player.rb.AddRelativeForce(force * runSpeed);

        // if player is jumping, switch to that state
        if (player.IsJumping) player.SwitchState(player.JumpState);
        if (player.SlopeState.IsOnSlope(player)) player.SwitchState(player.SlopeState);
        if (!player.IsSprinting) player.SwitchState(player.WalkState);
    }
    public override void OnCollisionEnterState(PlayerController player, Collision collision)
    {
    }
}
