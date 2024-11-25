using System;
using UnityEngine;

[Serializable]
public class PlayerRunState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        Debug.Log("started running");
    }

    public override void OnCollisionEnter(PlayerController player, Collision collision)
    {
    }

    public override void UpdateState(PlayerController player)
    {
    }
}
