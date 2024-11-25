using System;
using UnityEngine;

[Serializable]
public class PlayerWalkState : PlayerBaseState
{
    float walkSpeed = 10;

    public override void EnterState(PlayerController player)
    {
        Debug.Log("started walking");
    }

    public override void OnCollisionEnter(PlayerController player, Collision collision)
    {

    }

    public override void UpdateState(PlayerController player)
    {

        Vector2 movement = player.movement;
        Vector3 force = new(movement.x, 0f, movement.y);
        player.Rb.AddRelativeForce(force * walkSpeed);

        // if player is jumping, switch to that state
        if (player.IsJumping) player.SwitchState(player.JumpState);
    }
}
