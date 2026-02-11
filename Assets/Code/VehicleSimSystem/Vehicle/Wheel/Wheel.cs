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
        wheelSimPort.SetWheelTorque(ID, MathF.Abs(F_long) * config.Radius);
    }


    public void Simulate(float dt)
    {
        wheelOPS.suspensionForce = wheelSimPort.GetSuspensionForce(ID);

        Simlation(dt);
    }

    private float wheelOmega;
    private float wheelRPM;
    private float lngSlip;
    private float latSlip;

    private float F_long;

    private void Simlation(float dt)
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

        if (config.RecieveBrake)
        {   
            float tBrake = maxTireForce * lngSlip * vSimCtx.Brake;
            angularAcc = tBrake / config.Inertia;
            wheelOmega += MathF.Min(MathF.Abs(wheelOmega), MathF.Abs(angularAcc) * dt) * -MathF.Sign(wheelOmega);
        }

        wheelRPM = wheelOmega * 60 / (2 * MathF.PI);
        wheelOPS.wheelAngularVelocity = wheelOmega;

        CalculateLongSlip();
        F_long = lngSlip * maxTireForce;      
        wheelOPS.forwardForce = F_long;

        CalculateLatSlip();
        float F_lat = latSlip * maxTireForce;
        wheelOPS.lateralForce = F_lat;

        if (config.Steered)
            wheelOPS.wheelSteer = vSimCtx.Steering * 30f;
    }

    private void CalculateLatSlip()
    {
        float V_z = wheelIPS.contactPointVelocity.Z;
        float V_x = wheelIPS.contactPointVelocity.X;

        if (MathF.Abs(V_z) < 0.1f)
        {
            latSlip = 0f;
            return;
        }

        float slipAngle = MathF.Atan2(MathF.Abs(V_z), MathF.Abs(V_x));
        float latSlipRatio = slipAngle / (MathF.PI * 0.5f);
        latSlipRatio = MathF.Min(latSlipRatio, 1f);
        latSlipRatio = MathF.Max(latSlipRatio, 0f);

        latSlip = config.LatSlipCurve.Evaluate(latSlipRatio) * -MathF.Sign(wheelIPS.contactPointVelocity.Z);
    }   

    private void CalculateLongSlip()
    {
        float V_x = wheelIPS.contactPointVelocity.X;
        float denom = MathF.Max(MathF.Abs(V_x), MathF.Abs(wheelOmega * config.Radius));
        float lngSlipRatio = ((wheelOmega * config.Radius) - V_x) / MathF.Max(denom, 0.5f);
        lngSlip = config.LongSlipCurve.Evaluate(MathF.Abs(lngSlipRatio)) * MathF.Sign(lngSlipRatio);
    }
}

