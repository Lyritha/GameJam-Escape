using System;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpState", menuName = "Scriptable Objects/PlayerStates/JumpState")]
public class PlayerJumpState : PlayerBaseState
{
    [SerializeField] private float jumpStrength = 5;

    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }

    public override void EnterState(PlayerController player)
    {
        // Calculate velocity based on input and walking speed
        player.velocity += new Vector3(0, jumpStrength, 0);

        player.SwitchState(player.fallState);
    }

    public override void UpdateState(PlayerController player)
    {
    }

    public override void OnCollisionEnterState(PlayerController player, Collision collision)
    {
    }
}
