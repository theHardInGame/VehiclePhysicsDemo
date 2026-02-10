using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
internal sealed class WheelCollider : MonoBehaviour
{
    private ContactPoint[] contactPoints;

    public bool IsGrounded { get; private set; }

    [SerializeField] private string groundTag;

    private void OnCollisionStay(Collision collision)
    {
        List<ContactPoint> validPoints = new();

        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint pt = collision.contacts[i];
            if (pt.otherCollider.tag == groundTag)
            {
                validPoints.Add(pt);
            }
        }

        IsGrounded = validPoints.Count > 0;

        contactPoints = validPoints.ToArray();
    }

    public (Vector3 loc, Vector3 Dir) GetForceLocationAndDirection(float forceDirection)
    {
        Vector3 forceDir = new(forceDirection, 0, 0);
        ContactPoint bestPoint = contactPoints[0];

        for (int i = 0; i < contactPoints.Length; i++)
        {
            Vector3 pt = contactPoints[i].point;
            if (forceDirection >= 0)
            {
                if (Vector3.Dot(transform.InverseTransformDirection(pt - transform.position), forceDir) > Vector3.Dot(transform.InverseTransformDirection(bestPoint.point - transform.position), forceDir)) 
                    bestPoint = contactPoints[i];
            }
            else
            {
                if (Vector3.Dot(transform.InverseTransformDirection(pt - transform.position), forceDir) > Vector3.Dot(transform.InverseTransformDirection(bestPoint.point - transform.position), forceDir)) 
                    bestPoint = contactPoints[i];
            }
        }

        return (bestPoint.point, Vector3.ProjectOnPlane(transform.right, bestPoint.normal).normalized);
    }


}