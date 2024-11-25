using System;
using UnityEngine;

[Serializable]
public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        Debug.Log("started jumping");
    }

    public override void OnCollisionEnter(PlayerController player, Collision collision)
    {
    }

    public override void UpdateState(PlayerController player)
    {
    }
}
