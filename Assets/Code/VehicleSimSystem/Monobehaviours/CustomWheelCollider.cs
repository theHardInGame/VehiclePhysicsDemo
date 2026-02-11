using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
internal sealed class CustomWheelCollider : MonoBehaviour
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

    private void Update()
    {
        //Debug.DrawRay(transform.position, transform.right * 1000, Color.red);
        //Debug.DrawRay(transform.position, transform.forward * 1000, Color.blue);
        //Debug.DrawRay(transform.position + transform.up, Forward * 1000, Color.red);
        //Debug.DrawRay(transform.position + transform.up, Left * 1000, Color.blue);
    }

    public Vector3 Forward => transform.right;
    public Vector3 Left => transform.forward;

    public Vector3 GetForceLocAndDir(float forceSign, Vector3 forceAxis)
    {
        forceAxis = transform.TransformDirection(forceAxis);
        ContactPoint bestPoint = contactPoints[0];

        for (int i = 0; i < contactPoints.Length; i++)
        {
            Vector3 pt = contactPoints[i].point;
            if (forceSign >= 0)
            {
                if (Vector3.Dot(pt - transform.position, forceAxis) > Vector3.Dot(bestPoint.point - transform.position, forceAxis)) 
                    bestPoint = contactPoints[i];
            }
            else
            {
                if (Vector3.Dot(pt - transform.position, forceAxis) > Vector3.Dot(bestPoint.point - transform.position, forceAxis)) 
                    bestPoint = contactPoints[i];
            }
        }

        return bestPoint.point;
    }
}