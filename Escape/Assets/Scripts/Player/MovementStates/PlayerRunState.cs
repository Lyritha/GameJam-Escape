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
        // Get player input for movement
        Vector2 movement = player.Movement;

        // Calculate velocity based on input and walking speed
        player.velocity += Time.deltaTime * runSpeed * new Vector3(movement.x, 0f, movement.y);

        //check if you need to switch
        if (player.IsJumping) player.SwitchState(player.JumpState);
        if (!player.IsSprinting) player.SwitchState(player.WalkState);
    }
    public override void OnCollisionEnterState(PlayerController player, Collision collision)
    {
    }
}
