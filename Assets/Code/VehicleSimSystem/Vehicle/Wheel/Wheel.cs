using System;

internal sealed class Wheel : BaseVehicleComponent<WheelConfig>
{
    public Wheel(WheelConfig config, VehicleIOState vIOState, IWheelSimulationPort wheelPort) : base(config, vIOState)
    {
        if (!Guid.TryParse(config.ID, out Guid id))
        {
            throw new InvalidOperationException($"Invalid WheelConfig ID '{config.ID}' in {config.Name}");
        }
        
        wheelIPS = vIOState.GetWheelInputState(id);
        wheelOPS = vIOState.GetWheelOutputState(id);

        this.wheelSimPort = wheelPort;

        ID = wheelPort.RegisterWheel(id, config.Powered, config.Steered, config.Radius);

        this.vIOState = null;
    }

    private readonly int ID;
    private WheelInputState wheelIPS;
    private WheelOutputState wheelOPS;
    private IWheelSimulationPort wheelSimPort;

    

    public void Setup()
    {
        wheelSimPort.SetSpringLength(ID, wheelIPS.suspensionLength);
        wheelSimPort.SetSpringRelativeVelocity(ID, wheelIPS.springRelativeVelocity);
        wheelSimPort.SetWheelRPM(ID, wheelRPM);
        wheelSimPort.SetWheelTorque(ID, F_tire * config.Radius);
    }


    public void Simulate(float dt)
    {
        wheelOPS.suspensionForce = wheelSimPort.GetSuspensionForce(ID);

        DrivetrainSimlation(dt);
    }

    private float wheelOmega;
    private float wheelRPM;
    private float slip;
    private float mu;

    private float F_tire;

    private void DrivetrainSimlation(float dt)
    {   
        float maxTireForce = wheelOPS.suspensionForce;
        float tWheel = (wheelOmega - wheelIPS.contactPointVelocity.X / config.Radius) * config.Inertia / dt;
        tWheel = MathF.Min(tWheel, maxTireForce);
        tWheel = MathF.Max(tWheel, -maxTireForce);

        float angularAcc = -tWheel / config.Inertia;
        wheelOmega += angularAcc * dt;

        float tResistance = wheelOmega * wheelOPS.suspensionForce * config.RollingResistance;
        float tDrivetrain = wheelSimPort.GetDrivetrainTorque(ID);
        angularAcc = (tDrivetrain - tResistance) / config.Inertia;
        wheelOmega += angularAcc * dt;

        wheelRPM = wheelOmega * 60 / (2 * MathF.PI);
        wheelOPS.wheelAngularVelocity = wheelOmega;

        CalculateSlip();
        F_tire = CalculateTireForce();      
        wheelOPS.forwardForce = F_tire;
    }

    private void LateralResistance()
    {
        
    }

    private void CalculateSlip()
    {
        float V_x = wheelIPS.contactPointVelocity.X;
        float denom = MathF.Max(MathF.Abs(V_x), MathF.Abs(wheelOmega * config.Radius));
        slip = ((wheelOmega * config.Radius) - V_x) / MathF.Max(denom, 0.5f);
        mu = config.SlipCurve.Evaluate(MathF.Abs(slip));
    }

    private float CalculateTireForce()
    {
        float Fx = MathF.Sign(slip) * mu * wheelOPS.suspensionForce;
        return Fx;
    }

    

}

