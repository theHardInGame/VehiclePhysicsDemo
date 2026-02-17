using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
internal sealed class WheelMB : BaseInputComponent
{
    #region BaseInputComponent Implementation
    // ===================================
    // BaseInputComponent Implmenetation
    // ===================================

    protected override void OnControllerAwake()
    {
        wheelRBD = GetComponent<Rigidbody>();
        wheelRBD.mass = simIPort.Mass;
    }

    protected override void OnControllerEarlyUpdate(float dt)
    {
        ReadInput();
    }

    protected override void OnControllerLateUpdate(float dt)
    {
        WheelClamping();
        rollAngle -= Mathf.Rad2Deg * simIPort.WheelAngularVelocity * dt;
        SuspensionForce();
        AccelerationForce();
        LateralForce();
        Steer(dt);
    }
    #endregion

    #region WheelMB Methods
    // =================
    // WheelMB Methods
    // =================

    private ContactPoint[] contactPoints;

    [SerializeField] private Transform rotatingWheel;
    [SerializeField] private string groundTag;

    private bool isGrounded;
    private Rigidbody wheelRBD;

    private void ReadInput()
    {
        simIPort.SetIsGrounded(isGrounded);
    }

    /// <summary>
    /// Applies suspension force of wheel rbd.
    /// </summary>
    private void SuspensionForce()
    {
        wheelRBD.AddForce(-transform.up * simIPort.SuspensionForce, ForceMode.Force);
    }

    /// <summary>
    /// Applies acceleration force on vehicle rbd.
    /// </summary>
    private void AccelerationForce()
    {
        if (!isGrounded) return;
        
        float forwardForce = simIPort.ForwardForce;
        Vector3 LongLoc = GetForceLocation(forwardForce, new Vector3(forwardForce, 0, 0).normalized);
        Vector3 LongDir = transform.right;

        vehicleRBD.AddForceAtPosition(LongDir * forwardForce, LongLoc);
    }

    /// <summary>
    /// Applies lateral movement resistance on vehicle rbd.
    /// </summary>
    private void LateralForce()
    {
        if (!isGrounded) return;

        float lateralForce = simIPort.LateralForce;
        Vector3 LatLoc = GetForceLocation(lateralForce, new Vector3(0, 0, lateralForce).normalized);
        Vector3 LatDir = transform.forward;

        vehicleRBD.AddForceAtPosition(LatDir * lateralForce, LatLoc);
    }

    /// <summary>
    /// Set current steer angle to wheel.
    /// </summary>
    float lastSteerAngle;
    private void Steer(float dt)
    {
        float steerAngle = simIPort.WheelSteer;
        transform.localRotation = Quaternion.Euler(0, Mathf.MoveTowards(lastSteerAngle, steerAngle, (steerAngle - lastSteerAngle) * dt), 0);
        lastSteerAngle = steerAngle;
    }

    /// <summary>
    /// Finds best point to apply force on (furthermost point along the force axis in contact with ground).
    /// </summary>
    /// <param name="forceSign">Nature of force along forceDir</param>
    /// <param name="forceDir">Direction of force to be applied</param>
    /// <returns>Furthermost point along the forceDir</returns>
    private Vector3 GetForceLocation(float forceSign, Vector3 forceDir)
    {
        forceDir = transform.TransformDirection(forceDir);
        ContactPoint bestPoint = contactPoints[0];

        for (int i = 0; i < contactPoints.Length; i++)
        {
            Vector3 pt = contactPoints[i].point;
            if (forceSign >= 0)
            {
                if (Vector3.Dot(pt - transform.position, forceDir) > Vector3.Dot(bestPoint.point - transform.position, forceDir)) 
                    bestPoint = contactPoints[i];
            }
            else
            {
                if (Vector3.Dot(pt - transform.position, forceDir) > Vector3.Dot(bestPoint.point - transform.position, forceDir)) 
                    bestPoint = contactPoints[i];
            }
        }

        return bestPoint.point;
    }

    /// <summary>
    /// Clamps unnecessary movements of wheel.
    /// </summary>
    private void WheelClamping()
    {
        transform.localPosition = new Vector3(0f, transform.localPosition.y, 0f);
        transform.localRotation = Quaternion.identity;
    }
    #endregion

    #region Unity API
    // ===========
    // Unity API
    // ===========

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

        isGrounded = validPoints.Count > 0;

        contactPoints = validPoints.ToArray();
    }

    private float rollAngle;
    private float lastRollAngle;

    private void Update()
    {
        float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;

        float smoothAngle = Mathf.LerpAngle(
            lastRollAngle,
            rollAngle,
            alpha
        );
        
        rotatingWheel.localRotation = Quaternion.Euler(0f, 0f, smoothAngle);
        lastRollAngle = rollAngle;
    }
    #endregion
}