using System.Numerics;

internal interface ISimulationInputPort
{
    public void SetSuspensionLength(float length);
    public void SetContactPointVelocity(Vector3 velocity);
    public void SetSpringRelativeVelocity(float veloctiy);
    public void SetIsGrounded(bool isGrounded);

    public float ForwardForce { get; }
    public float LateralForce { get; }
    public float SuspensionForce { get; } 
    public float WheelAngularVelocity { get; }
    public float WheelSteer { get; }
    public float Mass { get; }
}

internal interface ISimulationOuputPort
{
    public void SetForwardForce(float force);
    public void SetLateralForce(float force);
    public void SetSuspensionForce(float force);
    public void SetWheelAngularVelocity(float velocity);
    public void SetWheelSteer(float steer);

    public float SuspensionLength { get; }
    public Vector3 ContactPointVelocity { get; }
    public float SpringRelativeVelocity { get; }
    public bool IsGrounded { get; }
}