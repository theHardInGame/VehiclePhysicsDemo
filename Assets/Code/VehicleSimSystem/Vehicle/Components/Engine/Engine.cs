using System;

internal sealed class Engine : BaseVehicleComponent<EngineConfig>, IDrivetrainComponent
{
    internal Engine(EngineConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
        this.vIOState = null;
        RPM = 0;
    }

    private float RPM;
    private float NetTorque => engineTorque - loadTorque;
    private float loadTorque;
    private float engineTorque;

    public float SimulateForwardTorque(float torqueIn, float dt)
    {
        engineTorque = config.RPMTorqueCurve.Evaluate(RPM) * vSimCtx.Throttle;
        return NetTorque;
    }

    public float SimulateBackwardTorque(float torqueIn, float dt)
    {
        loadTorque = torqueIn;
        RPM += NetTorque * dt * 60 / (2 * config.RotationalInertia * MathF.PI);
        
        RPM -= RPM * config.EngineDrag * dt;

        if (RPM < config.IdleRPM)
        {
            float idleError = config.IdleRPM - RPM;
            RPM += idleError * config.IdleRecoveryStrength * dt;
        }

        RPM = MathF.Max(RPM, 0f);
        RPM = MathF.Min(RPM, config.MaxRPM);

        vSimCtx.SetEngineRPM(RPM);

        return 0f;
    }
}