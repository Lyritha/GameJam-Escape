using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FallState", menuName = "Scriptable Objects/PlayerStates/FallState")]
public class PlayerFallState : PlayerBaseState
{
    [SerializeField] private LayerMask groundMask;

    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }

    public override void EnterState(PlayerController player)
    {
        Debug.Log("started falling");
    }

    public override void UpdateState(PlayerController player)
    {
    }

    public override void OnCollisionEnterState(PlayerController player, Collision collision)
    {
        Debug.Log("landed");

        if (((1 << collision.gameObject.layer) & groundMask) != 0)
        {
            player.SwitchState(player.WalkState);
        }
    }
}
