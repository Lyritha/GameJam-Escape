using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

[Serializable]
public class CollideSlide : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private CapsuleCollider coll;
    [SerializeField] private GroundChecker ground;

    [SerializeField] private float maxSlopeAngle = 55;
    [SerializeField] private float maxStepHeight = 0.1f;

    private const int MaxDepth = 20;
    private const float SkinWidth = 0.015f;

    private Vector3 _p1;
    private Vector3 _p2;
    private float _radius;

    // saved values for gizmos, don't use for actual calculations, keep parameters limited to their needed scopes
    Vector3 gizmoVelocity = Vector3.zero;

    private void Update()
    {
        //Bounds bounds;
        //bounds = coll.bounds;
        //bounds.Expand(-2 * SkinWidth);
    }

    /// <summary>
    /// Main method to handle collision and sliding for an object.
    /// </summary>
    public Vector3 CollideAndSlide(Vector3 velocity, Vector3 position, int depth, bool gravityPass, Vector3 initialVelocity)
    {
        gizmoVelocity = velocity;

        if (depth >= MaxDepth) return Vector3.zero; // Prevent infinite recursion

        float distance = velocity.magnitude + SkinWidth;
        UpdateCapsuleData(position);

        if (Physics.CapsuleCast(_p1, _p2, _radius - SkinWidth, velocity.normalized, out RaycastHit hit, distance, collisionMask))
        {
            Vector3 snapToSurface = velocity.normalized * (hit.distance - SkinWidth);
            Vector3 remainingVelocity = velocity - snapToSurface;

            snapToSurface = snapToSurface.magnitude <= SkinWidth ? Vector3.zero : snapToSurface;

            // angle of the surface that got collided with
            float angle = Vector3.Angle(Vector3.up, hit.normal);

            // check if it is a slope, or something else 
            if (angle <= maxSlopeAngle)
            {
                // if this is a gravity pass, just snap to surface instead of applying gravity
                if (gravityPass) return snapToSurface;

                // project the velocity along the normal of the collided face
                remainingVelocity = ProjectAndScale(remainingVelocity, hit.normal);
            }
            else
            {
                // create poin at which the "feet" are positioned
                Vector3 footPosition = (_p1 + Vector3.down * _radius) + (Vector3.up * SkinWidth);

                // DEBUG ray to show where it's currently checking for feet collision
                Vector3 horizontalVel = new(velocity.x, 0, velocity.z);
                Debug.DrawLine(footPosition, footPosition + horizontalVel * 100);

                //handle step by adding to the remaining velocity, if not a step add friction
                if (IsStep(footPosition, velocity, hit) && !gravityPass)
                {
                    remainingVelocity += Vector3.up * (hit.point.y - footPosition.y);
                }
                else
                {
                    float scale = GetWallFriction(hit, initialVelocity);

                    if (ground.isGrounded && !gravityPass)
                        remainingVelocity = ProjectAndScale(new(remainingVelocity.x, 0, remainingVelocity.z), new(hit.normal.x, 0, hit.normal.z)) * scale;
                    else
                        remainingVelocity = ProjectAndScale(remainingVelocity, hit.normal) * scale;
                }
            }

            return snapToSurface + CollideAndSlide(remainingVelocity, position + snapToSurface, depth + 1, gravityPass, initialVelocity);
        }

        return velocity; // No collision, return original velocity
    }

    private void UpdateCapsuleData(Vector3 position)
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

        // Check if raycast hits and the angle is acceptable, if not, then it isn't a step
        if (!Physics.Raycast(footPos, horizontalVel, out RaycastHit footHit, 10, collisionMask) ||
            Vector3.Angle(Vector3.up, footHit.normal) < 90)
            return false;

        // Check if the step is within the allowed vertical distance and the horizontal velocity is sufficient
        float verticalDistance = hit.point.y - footPos.y;
        if (verticalDistance <= 0.001f || verticalDistance > maxStepHeight)
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

    private Vector3 ProjectAndScale(Vector3 velocity, Vector3 normal)
    {
        velocity = Vector3.ProjectOnPlane(velocity, normal);
        return velocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_p1, _radius);
        Gizmos.DrawWireSphere(_p2, _radius);

        if (Physics.CapsuleCast(_p1, _p2, _radius, gizmoVelocity.normalized, out RaycastHit hit, 10, collisionMask))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hit.point, hit.point + hit.normal * 3);
        }
    }
}
