using UnityEngine;
using System;
using Unity.Collections;
using TMPro;

internal sealed class WheelMB : MonoBehaviour
{
    [SerializeField, ReadOnly] private string id;
    [SerializeField] private Rigidbody wheelDisplace;
    [SerializeField] private Transform wheelRotate;
    [SerializeField] private CustomWheelCollider wheelCollider;
    [SerializeField] private Rigidbody body;
    [SerializeField] private TextMeshProUGUI debugText;
    public Guid ID { get; private set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif

    public WheelInputState WheelIPS
    {
        get
        {
            wheelIPDNT ??= new();
            return wheelIPDNT;
        }
    }

    public WheelOutputState WheelOPS
    {
        get
        {
            wheelOPDNT ??= new();
            return wheelOPDNT;
        }
    }

    private WheelInputState wheelIPDNT;
    private WheelOutputState wheelOPDNT;

    internal void ControllerAwake(float mass)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Empty Guid");
            return;
        }

        if (Guid.TryParse(id, out Guid guid)) { ID = guid; }

        wheelDisplace.mass = mass;
    }

    Vector3 lastFramPosition;

    internal void WheelIPSRead()
    {
        Vector3 wheelVel = wheelDisplace.transform.InverseTransformDirection(wheelDisplace.transform.position - lastFramPosition);
        debugText.text = $"Wheel vel: {wheelVel}";
        WheelIPS.contactPointVelocity = new System.Numerics.Vector3(wheelVel.x, wheelVel.y, wheelVel.z) / Time.fixedDeltaTime;
        lastFramPosition = wheelDisplace.transform.position;

        WheelIPS.suspensionLength = (transform.position - wheelDisplace.transform.position).magnitude;

        Vector3 axis = (wheelDisplace.transform.position - transform.position).normalized;

        Vector3 rWheel = wheelDisplace.transform.position - wheelDisplace.worldCenterOfMass;
        Vector3 rBody  = transform.position - body.worldCenterOfMass;

        Vector3 vPointWheel = wheelVel + Vector3.Cross(wheelDisplace.angularVelocity, rWheel);
        Vector3 vPointBody  = body.linearVelocity + Vector3.Cross(body.angularVelocity, rBody);

        WheelIPS.springRelativeVelocity = Vector3.Dot(vPointWheel - vPointBody, axis);

        //WheelIPS.IsGrounded = wheelCollider.IsGrounded;
    }

    float rollAngle;
    float lastRollAngle;

    float steerAngle;
    float lastSteerAngle;

    internal void ControllerUpdate(float dt)
    {
        WheelClamping();
        SuspensionForce();
        TangentForces(dt);
        RotateWheel(dt);
        Steer();
    }

    private void FixedUpdate()
    {

    }

    private void Update()
    {
        float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;

        float smoothAngle = Mathf.LerpAngle(
            lastRollAngle,
            rollAngle,
            alpha
        );

        wheelRotate.localRotation = Quaternion.Euler(0f, 0f, smoothAngle);

        lastRollAngle = rollAngle;
    }

    private void RotateWheel(float dt)
    {
        rollAngle -= Mathf.Rad2Deg * WheelOPS.wheelAngularVelocity * dt;
    }

    private void SuspensionForce()
    {
        wheelDisplace.AddForce(-wheelDisplace.transform.up * WheelOPS.suspensionForce, ForceMode.Force);
        body.AddForceAtPosition(transform.up * WheelOPS.suspensionForce, transform.position, ForceMode.Force);

        debugText.text = $"Suspension Force: {WheelOPS.suspensionForce:F2} ";
    }


    private void TangentForces(float dt)
    {
        wheelCollider.transform.localRotation = Quaternion.Euler(0, Mathf.MoveTowards(lastSteerAngle, steerAngle, (steerAngle - lastSteerAngle) * dt), 0);
        lastSteerAngle = steerAngle;
        
        if (!wheelCollider.IsGrounded) return;
        
        float forwardForce = WheelOPS.forwardForce;
        Vector3 LongLoc = wheelCollider.GetForceLocAndDir(forwardForce, new Vector3(forwardForce, 0, 0).normalized);
        Vector3 LongDir = wheelCollider.transform.right;

        body.AddForceAtPosition(LongDir * forwardForce, LongLoc);
        Debug.DrawRay(LongLoc, LongDir * forwardForce, Color.red);

        float lateralForce = WheelOPS.lateralForce;
        Vector3 LatLoc = wheelCollider.GetForceLocAndDir(lateralForce, new Vector3(0, 0, lateralForce).normalized);
        Vector3 LatDir = wheelCollider.transform.forward;

        body.AddForceAtPosition(LatDir * lateralForce, LatLoc);
        Debug.DrawRay(LatLoc, LatDir * lateralForce, Color.blue);
        

        debugText.text += $"Long Force: {WheelOPS.forwardForce}";
        debugText.text += $"Lat Force: {WheelOPS.lateralForce}";
    }

    private void Steer()
    {
        steerAngle = WheelOPS.wheelSteer;
    }


    private void WheelClamping()
    {
        wheelDisplace.transform.localPosition = new Vector3(0f, wheelDisplace.transform.localPosition.y, 0f);
        wheelDisplace.transform.localRotation = Quaternion.identity;
    }
}