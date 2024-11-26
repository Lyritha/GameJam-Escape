using System;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpState", menuName = "Scriptable Objects/PlayerStates/JumpState")]
public class PlayerJumpState : PlayerBaseState
{
    [SerializeField] private float jumpStrength = 5;

    [SerializeField] private LayerMask groundMask;

    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }

    public override void EnterState(PlayerController player)
    {
        player.rb.AddForce(0, jumpStrength, 0 , ForceMode.Impulse);
    }

    public override void UpdateState(PlayerController player)
    {
    }

    public override void OnCollisionEnterState(PlayerController player, Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask) != 0)
        {
            player.SwitchState(player.WalkState);
        }
    }
}
