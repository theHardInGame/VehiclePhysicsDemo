using System.Numerics;

/// <summary>
/// Input and Output paramters for each wheel.
/// </summary>
internal sealed class SimulationIOParamters: ISimulationInputPort, ISimulationOuputPort
{
    public SimulationIOParamters(float mass)
    {
        Mass = mass;
    }

    // Shared Context
    public float Mass { get; private set; }

    // Input Parameters
    public float SuspensionLength { get; private set; }
    public Vector3 ContactPointVelocity { get; private set; }
    public float SpringRelativeVelocity { get; private set; }
    public bool IsGrounded { get; private set; }

    // Output Parameters
    public float ForwardForce { get; private set; }
    public float LateralForce { get; private set; }
    public float SuspensionForce { get; private set; }
    public float WheelAngularVelocity { get; private set; }
    public float WheelSteer { get; private set; }

    #region ISimulationInputPort
    public void SetSuspensionLength(float length) { SuspensionLength = length; }
    public void SetContactPointVelocity(Vector3 velocity) { ContactPointVelocity = velocity; }
    public void SetSpringRelativeVelocity(float veloctiy) { SpringRelativeVelocity = veloctiy; }
    public void SetIsGrounded(bool grounded) { IsGrounded = grounded; }
    #endregion

    #region ISimulationOutputPort
    public void SetForwardForce(float force) { ForwardForce = force; }
    public void SetLateralForce(float force) { LateralForce = force; }
    public void SetSuspensionForce(float force) { SuspensionForce = force; }
    public void SetWheelAngularVelocity(float velocity) { WheelAngularVelocity = velocity; }
    public void SetWheelSteer(float steer) { WheelSteer = steer; }
    #endregion
}