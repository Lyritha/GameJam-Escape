using System;
using UnityEngine;

public abstract class PlayerBaseState : ScriptableObject
{
    public abstract void OnEnable();
    public abstract void OnDisable();

    public abstract void EnterState(PlayerController player);

    public abstract void UpdateState(PlayerController player);

    public abstract void OnCollisionEnterState(PlayerController player, Collision collision);
}
