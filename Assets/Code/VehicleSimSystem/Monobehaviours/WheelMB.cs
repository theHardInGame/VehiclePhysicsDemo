using UnityEngine;
using System;
using Unity.Collections;
using TMPro;

internal sealed class WheelMB : MonoBehaviour
{
    [SerializeField, ReadOnly] private string id;
    [SerializeField] private Rigidbody wheelDisplace;
    [SerializeField] private Transform wheelRotate;
    [SerializeField] private WheelCollider wheelCollider;
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
        Vector3 wheelVel = wheelDisplace.transform.position - lastFramPosition;
        WheelIPS.contactPointVelocity = new System.Numerics.Vector3(wheelVel.x, wheelVel.y, wheelVel.z) / Time.fixedDeltaTime;
        lastFramPosition = wheelDisplace.transform.position;

        WheelIPS.suspensionLength = (transform.position - wheelDisplace.transform.position).magnitude;

        Vector3 axis = (wheelDisplace.transform.position - transform.position).normalized;

        Vector3 rWheel = wheelDisplace.transform.position - wheelDisplace.worldCenterOfMass;
        Vector3 rBody  = transform.position - body.worldCenterOfMass;

        Vector3 vPointWheel = /*wheelDisplace.linearVelocity*/ wheelVel + Vector3.Cross(wheelDisplace.angularVelocity, rWheel);
        Vector3 vPointBody  = body.linearVelocity + Vector3.Cross(body.angularVelocity, rBody);

        WheelIPS.springRelativeVelocity = Vector3.Dot(vPointWheel - vPointBody, axis);

        //WheelIPS.IsGrounded = wheelCollider.IsGrounded;
    }

    float wheelAngle;
    float lastAngle;

    internal void ControllerUpdate(float dt)
    {
        WheelClamping();
        SuspensionForce();
        LongitudinalForce();
        RotateWheel(dt);
        //LateralForce();
    }

    private void Update()
    {
        float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;

        float smoothAngle = Mathf.LerpAngle(
            lastAngle,
            wheelAngle,
            alpha
        );

        lastAngle = wheelAngle;

        wheelRotate.localRotation = Quaternion.Euler(0f, 0f, smoothAngle);
    }

    private void RotateWheel(float dt)
    {
        wheelAngle -= Mathf.Rad2Deg * WheelOPS.wheelAngularVelocity * dt;
    }

    private void SuspensionForce()
    {
        // Apply equal and opposite forces
        wheelDisplace.AddForce(-wheelDisplace.transform.up * WheelOPS.suspensionForce, ForceMode.Force);
        body.AddForceAtPosition(transform.up * WheelOPS.suspensionForce, transform.position, ForceMode.Force);

        debugText.text = $"Suspension Force: {WheelOPS.suspensionForce:F2} ";
    }

    private void LateralForce()
    {
        // Apply lateral force at contact point
        Vector3 lateralDirection = transform.forward; // Assuming right is the lateral direction
        wheelDisplace.AddForce(-lateralDirection * WheelOPS.lateralForce, ForceMode.Force);

        debugText.text += $"Lateral Force: {WheelOPS.lateralForce:F2}";
    }

    private void LongitudinalForce()
    {
        if (!wheelCollider.IsGrounded) return;
        
        float forwardForce = WheelOPS.forwardForce;
        (Vector3 Loc, Vector3 Dir) = wheelCollider.GetForceLocationAndDirection(MathF.Sign(forwardForce));

        body.AddForceAtPosition(Dir * WheelOPS.forwardForce, Loc);

        Debug.DrawRay(Loc, Dir * forwardForce, Color.red);

        debugText.text += $"Long Force: {WheelOPS.forwardForce}";
        debugText.text += $"Lat Force: {WheelOPS.lateralForce}";
    }

    private void WheelClamping()
    {
        wheelDisplace.transform.localPosition = new Vector3(0f, wheelDisplace.transform.localPosition.y, 0f);
        wheelDisplace.transform.localRotation = Quaternion.identity;
    }
}