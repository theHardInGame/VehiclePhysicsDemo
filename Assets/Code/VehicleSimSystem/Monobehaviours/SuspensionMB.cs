using UnityEngine;

internal sealed class SuspensionMB : BaseInputComponent
{
    [SerializeField] private Transform suspensionJoint;
    [SerializeField] private Transform wheelJoint;
    [SerializeField] private Rigidbody wheelRBD;
    protected override void OnControllerAwake()
    {
        
    }

    protected override void OnControllerEarlyUpdate(float dt)
    {
        ReadInput();
    }

    protected override void OnControllerLateUpdate(float dt)
    {
        SuspensionForce();
    }

    private Vector3 lastFramePosition;
    private void ReadInput()
    {
        Vector3 wheelVel = transform.InverseTransformDirection(wheelJoint.transform.position - lastFramePosition);
        simIPort.SetContactPointVelocity(new System.Numerics.Vector3(wheelVel.x, wheelVel.y, wheelVel.z) / Time.fixedDeltaTime);
        lastFramePosition = wheelJoint.transform.position;

        simIPort.SetSuspensionLength((wheelJoint.position - suspensionJoint.position).magnitude);

        Vector3 axis = (wheelJoint.transform.position - suspensionJoint.transform.position).normalized;

        Vector3 rWheel = wheelJoint.transform.position - wheelRBD.worldCenterOfMass;
        Vector3 rBody  = transform.position - vehicleRBD.worldCenterOfMass;

        Vector3 vPointWheel = wheelVel + Vector3.Cross(wheelRBD.angularVelocity, rWheel);
        Vector3 vPointBody  = vehicleRBD.linearVelocity + Vector3.Cross(vehicleRBD.angularVelocity, rBody);

        simIPort.SetSpringRelativeVelocity(Vector3.Dot(vPointWheel - vPointBody, axis));
    }

    private void SuspensionForce()
    {
        vehicleRBD.AddForceAtPosition(vehicleRBD.transform.up * simIPort.SuspensionForce, transform.position, ForceMode.Force);
    }
}