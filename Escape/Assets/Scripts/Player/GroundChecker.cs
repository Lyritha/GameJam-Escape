using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private CapsuleCollider coll;

    [SerializeField] private float groundCheckOffset = 0.5f;
    [SerializeField] private float groundCheckRadius = 0.5f;

    public bool isGrounded = false;
    private Vector3 groundCheckPosition = Vector3.zero;

    /// <summary>
    /// Every FixedUpdate, check if the object is grounded
    /// </summary>
    private void FixedUpdate()
    {
        // Calculate the ground check position
        groundCheckPosition = transform.position + coll.center - Vector3.up * (coll.height / 2 - groundCheckOffset);

        bool circleGround = Physics.OverlapSphere(groundCheckPosition, groundCheckRadius, collisionMask).Length > 0;
        bool rayGround = Physics.Raycast(groundCheckPosition, Vector3.down, groundCheckRadius + 0.1f, collisionMask);

        isGrounded = circleGround || rayGround;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + coll.center - Vector3.up * (coll.height / 2 - groundCheckOffset), groundCheckRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(groundCheckPosition, Vector3.down);
    }
}
