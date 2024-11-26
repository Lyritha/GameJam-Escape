using System;
using UnityEngine;

[Serializable]
public class CollideSlide : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private CapsuleCollider coll;

    [SerializeField] private float maxSlopeAngle = 55;
    [SerializeField] private float maxStepHeight = 0.1f;

    private const int MaxDepth = 5;
    private const float SkinWidth = 0.015f;

    private Vector3 _p1;
    private Vector3 _p2;
    private float _radius;

    /// <summary>
    /// Main method to handle collision and sliding for an object.
    /// </summary>
    public Vector3 CollideAndSlide(Vector3 velocity, Vector3 position, int depth, bool gravityPass, Vector3 initialVelocity)
    {
        if (depth >= MaxDepth) return Vector3.zero; // Prevent infinite recursion

        float distance = velocity.magnitude + SkinWidth;
        SetCapsuleInfo(position);

        if (Physics.CapsuleCast(_p1, _p2, _radius, velocity.normalized, out RaycastHit hit, distance, collisionMask))
        {
            Vector3 snapToSurface = velocity.normalized * (hit.distance - SkinWidth);
            Vector3 remainingVelocity = velocity - snapToSurface;

            snapToSurface = snapToSurface.magnitude <= SkinWidth ? Vector3.zero : snapToSurface;

            float angle = Vector3.Angle(Vector3.up, hit.normal);

            if (angle <= maxSlopeAngle)
            {
                if (gravityPass)
                    return snapToSurface;

                remainingVelocity = ProjectOnPlane(remainingVelocity, hit.normal);
            }
            else
            {
                Vector3 footPosition = _p1 + Vector3.down * _radius;

                if (IsStep(footPosition, velocity, hit))
                {
                    Vector3 stepAdjustment = Vector3.up * (hit.point.y - footPosition.y);
                    return stepAdjustment + CollideAndSlide(remainingVelocity, position + stepAdjustment, 0, false, initialVelocity);
                }

                float frictionScale = GetWallFriction(hit, initialVelocity);
                remainingVelocity = ProjectOnPlane(remainingVelocity, hit.normal) * frictionScale;
            }

            return snapToSurface + CollideAndSlide(remainingVelocity, position + snapToSurface, depth + 1, gravityPass, initialVelocity);
        }

        return velocity; // No collision, return original velocity
    }

    private void SetCapsuleInfo(Vector3 position)
    {
        Vector3 collCenter = position + coll.center;
        _radius = coll.radius;
        Vector3 offset = Vector3.up * ((coll.height / 2) - _radius);

        _p1 = collCenter - offset;
        _p2 = collCenter + offset;
    }

    private bool IsStep(Vector3 footPos, Vector3 vel, RaycastHit hit)
    {
        Vector3 horizontalVel = new(vel.x, 0, vel.z);

        Debug.DrawRay(footPos, horizontalVel);

        // Check if raycast hits and the angle is acceptable, if not, then it isn't a step
        if (!Physics.Raycast(footPos, horizontalVel, out RaycastHit footHit, 10, collisionMask) ||
            Vector3.Angle(Vector3.up, footHit.normal) < 90)
            return false;

        // Check if the step is within the allowed vertical distance and the horizontal velocity is sufficient
        float verticalDistance = hit.point.y - footPos.y;
        if (verticalDistance <= 0 || verticalDistance > maxStepHeight || horizontalVel.magnitude <= 0.05f)
            return false;

        // Calculate the direction to the step, ignoring the vertical component (y-axis)
        Vector3 toStep = hit.point - footPos;
        toStep.y = 0;

        // Use horizontalVel to check if the velocity is aligned with the step direction
        return Vector3.Dot(horizontalVel.normalized, toStep.normalized) > 0.1f;
    }

    private float GetWallFriction(RaycastHit hit, Vector3 initialVelocity)
    {
        Vector3 hitNormalHorizontal = new Vector3(hit.normal.x, 0, hit.normal.z).normalized;
        Vector3 initialVelocityHorizontal = new Vector3(initialVelocity.x, 0, initialVelocity.z).normalized;

        return 1 - Vector3.Dot(hitNormalHorizontal, -initialVelocityHorizontal);
    }

    private Vector3 ProjectOnPlane(Vector3 velocity, Vector3 normal)
    {
        return Vector3.ProjectOnPlane(velocity, normal).normalized * velocity.magnitude;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_p1, _radius);
        Gizmos.DrawWireSphere(_p2, _radius);

        if (Physics.CapsuleCast(_p1, _p2, _radius, Vector3.forward, out RaycastHit hit, 10, collisionMask))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hit.point, hit.point + hit.normal * 3);
        }
    }
}
