using System;
using System.Collections.Generic;

internal sealed class Wheel : BaseVehicleComponent<WheelConfig>
{
    public Wheel(WheelConfig config, VehicleSimulationContext vSimCtx, IWheelPort wheelPort) : base(config, vSimCtx)
    {
        if (!Guid.TryParse(config.ID, out Guid id))
        {
            throw new InvalidOperationException($"Invalid WheelConfig ID '{config.ID}' in {config.Name}");
        }

        if (!vSimCtx.simOPorts.TryGetValue(id, out simOutPort))
        {
            throw new Exception($"Could not link SimulationIO to Wheel Name: {config.Name}");
        }

        this.wheelPort = wheelPort;

        ID = wheelPort.RegisterWheel(id, config.Powered, config.Steered, config.Radius);
    }

    private readonly int ID;
    private ISimulationOuputPort simOutPort;
    private IWheelPort wheelPort;

    

    public void Setup()
    {
        wheelPort.SetSpringLength(ID, simOutPort.SuspensionLength);
        wheelPort.SetSpringRelativeVelocity(ID, simOutPort.SpringRelativeVelocity);
        wheelPort.SetWheelRPM(ID, wheelRPM);
        wheelPort.SetWheelTorque(ID, MathF.Abs(F_long) * config.Radius);
    }


    public void Simulate(float dt)
    {
        simOutPort.SetSuspensionForce(F_z = wheelPort.GetSuspensionForce(ID));
        Simlation(dt);
    }

    private float wheelOmega;
    private float wheelRPM;
    private float lngSlip;
    private float latSlip;

    private float F_long;
    private float F_z;

    private void Simlation(float dt)
    {   
        float maxTireForce = F_z;
        float tWheel = (wheelOmega - simOutPort.ContactPointVelocity.X / config.Radius) * config.Inertia / dt;
        tWheel = MathF.Min(tWheel, maxTireForce);
        tWheel = MathF.Max(tWheel, -maxTireForce);

        float angularAcc = -tWheel / config.Inertia;
        wheelOmega += angularAcc * dt;

        float tResistance = wheelOmega * F_z * config.RollingResistance;
        float tDrivetrain = wheelPort.GetDrivetrainTorque(ID);
        angularAcc = (tDrivetrain - tResistance) / config.Inertia;
        wheelOmega += angularAcc * dt;

        if (config.RecieveBrake)
        {   
            float tBrake = maxTireForce * lngSlip * vSimCtx.Brake;
            angularAcc = tBrake / config.Inertia;
            wheelOmega += MathF.Min(MathF.Abs(wheelOmega), MathF.Abs(angularAcc) * dt) * -MathF.Sign(wheelOmega);
        }

        wheelRPM = wheelOmega * 60 / (2 * MathF.PI);
        simOutPort.SetWheelAngularVelocity(wheelOmega);

        CalculateLongSlip();
        F_long = lngSlip * maxTireForce;      
        simOutPort.SetForwardForce(F_long);

        CalculateLatSlip();
        float F_lat = latSlip * maxTireForce;
        simOutPort.SetLateralForce(F_lat);

        if (config.Steered)
            simOutPort.SetWheelSteer(vSimCtx.Steering * 30f);
    }

    private void CalculateLatSlip()
    {
        float V_z = simOutPort.ContactPointVelocity.Z;
        float V_x = simOutPort.ContactPointVelocity.X;

        if (MathF.Abs(V_z) < 0.1f)
        {
            latSlip = 0f;
            return;
        }

        float slipAngle = MathF.Atan2(MathF.Abs(V_z), MathF.Abs(V_x));
        float latSlipRatio = slipAngle / (MathF.PI * 0.5f);
        latSlipRatio = MathF.Min(latSlipRatio, 1f);
        latSlipRatio = MathF.Max(latSlipRatio, 0f);

        latSlip = config.LatSlipCurve.Evaluate(latSlipRatio) * -MathF.Sign(V_z);
    }   

    private void CalculateLongSlip()
    {
        float V_x = simOutPort.ContactPointVelocity.X;
        float denom = MathF.Max(MathF.Abs(V_x), MathF.Abs(wheelOmega * config.Radius));
        float lngSlipRatio = ((wheelOmega * config.Radius) - V_x) / MathF.Max(denom, 0.5f);
        lngSlip = config.LongSlipCurve.Evaluate(MathF.Abs(lngSlipRatio)) * MathF.Sign(lngSlipRatio);
    }
}

