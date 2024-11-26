using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WalkSlopeState", menuName = "Scriptable Objects/PlayerStates/WalkSlopeState")]
public class PlayerWalkSlopeState : PlayerBaseState
{
    [SerializeField] private float walkSlopeSpeed = 3;
    private RaycastHit slopeHit;

    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }


    public override void EnterState(PlayerController player)
    {
        Debug.Log("Slope go brrr");
    }


    public override void UpdateState(PlayerController player)
    {
        Vector2 movement = player.Movement;
        Vector3 force = new Vector3(movement.x, 0f, movement.y) * (Time.deltaTime * 100);
        force = player.transform.TransformDirection(force);
        force = SlopeDir(force);

        player.rb.AddForce(force * walkSlopeSpeed);

        if (!IsOnSlope(player)) player.SwitchState(player.WalkState);
    }

    public bool IsOnSlope(PlayerController player)
    {
        Ray ray = new(player.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out slopeHit))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle != 0) return true;
        }

        return false;
    }

    private Vector3 SlopeDir(Vector3 dir)
    {
        return Vector3.ProjectOnPlane(dir, slopeHit.normal);
    }

    public override void OnCollisionEnterState(PlayerController player, Collision collision)
    {

    }
}
