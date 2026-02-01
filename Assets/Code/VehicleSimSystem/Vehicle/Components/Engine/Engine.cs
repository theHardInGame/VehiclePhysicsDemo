using UnityEngine;

internal sealed class Engine : BaseVehicleComponent<EngineConfig>, IDrivetrainComponent
{
    internal Engine(EngineConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
        rpm = config.idleRPM;
    }

    private float rpm;
    private float loadTorque;
    private float power;


    private float Throttle => vIOState.vehicleCommands.GetThrottle();


    private float GetTorque()
    {
        float omega = rpm * Mathf.Deg2Rad * (1/60);

        if (omega <= 0f) return 0f;

        power = config.PowerCurve.Evaluate(rpm) * Throttle;
        return power/omega;
    }

    private void EngineCycle(float dt)
    {
        float torque = GetTorque();
        float frictionTorque = config.Friction * rpm;

        float netTorque = torque - loadTorque - frictionTorque;
        float angAcc = netTorque / config.Inertia;

        rpm += angAcc * dt;

        if (rpm < 0f) rpm = 0f;

        rpm = Mathf.Clamp(rpm, 0f, config.maxRPM);
    }

    private void RPMRecovery(float dt)
    {
        if (Mathf.Approximately(Throttle, 0))
        {
            rpm = Mathf.MoveTowards(rpm, config.idleRPM, 10 * dt);
        }
    }

    #region Drivetrain Module Interface
    // ============================================
    // IDrivetrainModule Interface Implementation
    // ============================================

    public ForwardState Forward(ForwardState input, float tick)
    {
        EngineCycle(tick);
        RPMRecovery(tick);

        input.power = this.power;
        input.rpm = rpm;
        input.torque = GetTorque();
        return input;
    }

    public BackwardState Backward(BackwardState input, float tick)
    {
        loadTorque = input.loadTorque;
        rpm = input.rpm;
        return input;
    }
    #endregion
}