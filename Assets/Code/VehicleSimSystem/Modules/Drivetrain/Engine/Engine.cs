using UnityEngine;

public sealed class Engine : BaseVehicleModule<EngineConfig>, IDrivetrainModule
{
    public Engine(EngineConfig config) : base(config)
    {
        rpm = config.idleRPM;
    }

    private float rpm;
    private float loadTorque;
    private float throttle;
    private float power;

    public void SetThrottle(float t)
    {
        throttle = t;
    }

    private float GetTorque()
    {
        float omega = rpm * Mathf.Deg2Rad * (1/60);

        if (omega <= 0f) return 0f;

        power = config.PowerCurve.Evaluate(rpm) * throttle;
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

    public override void OnFixedUpdate(float fixedDeltaTime)
    {
        base.OnFixedUpdate(fixedDeltaTime);
        EngineCycle(fixedDeltaTime);

        if (Mathf.Approximately(throttle, 0))
        {
            rpm = Mathf.MoveTowards(rpm, config.idleRPM, 10 * fixedDeltaTime);
        }
    }



    // ============================================
    // IDrivetrainModule Interface Implementation
    // ============================================

    ForwardState IDrivetrainModule.Forward(ForwardState input)
    {
        input.power = this.power;
        input.rpm = rpm;
        input.torque = GetTorque();
        return input;
    }

    BackwardState IDrivetrainModule.Backward(BackwardState input)
    {
        loadTorque = input.loadTorque;
        rpm = input.rpm;
        return input;
    }
}