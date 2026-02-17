using System;

internal sealed class Suspension : BaseVehicleComponent<SuspensionConfig>
{
    public Suspension(SuspensionConfig config, VehicleSimulationContext vSimCtx) : base(config, vSimCtx)
    {
        lastFrameLength = config.RestLength;
    }

    private float restLength => config.RestLength;
    private float springRate => config.SpringRate;
    private float damperRate => config.DamperRate;
    private float wheelMass => config.WheelMass;

    private float lastFrameLength;

    public float Simulate(float currentLength, float springRelativeVelocity, float dt)
    {
        // Clamp length to valid range
        currentLength = Math.Clamp(currentLength, config.MinLength, config.MaxLength);
        
        // Spring force (positive when compressed)
        float compression = restLength - currentLength;
        float springForce = springRate * compression;

        // Damper velocity and force (opposes motion)
        float velocity = (currentLength - lastFrameLength) / dt;
        float damperForce = -damperRate * springRelativeVelocity / wheelMass; // Scale by mass for stability
        
        lastFrameLength = currentLength;

        // Total force: clamp to prevent runaway
        float totalForce = springForce + damperForce;
        return Math.Clamp(totalForce, -50000f, 50000f);
    }
}