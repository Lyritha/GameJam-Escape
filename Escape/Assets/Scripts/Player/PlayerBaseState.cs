using System;
using UnityEngine;

[Serializable]
public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerController player);

    public abstract void UpdateState(PlayerController player);

    public abstract void OnCollisionEnter(PlayerController player, Collision collision);
}
