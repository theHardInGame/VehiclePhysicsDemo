using System;

internal sealed class Suspension : BaseVehicleComponent<SuspensionConfig>
{
    public Suspension(SuspensionConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
        currentLength = config.RestLength;
        gravity = vIOState.vSimCtx.GetGravity();
    }

    public int ID;

    private float gravity;

    private float currentLength;
    private float lastFrameLength;
    private float currentVelocity;
    private float lastFrameVelocity;

    private bool isGrounded;
    private float springForce;
    private float damperForce;
    private float verticalLoad;

    private float verticalWheelVelocity;
    

    public SuspensionOutputData Simulate(SuspensionInputData input, float dt)
    {
        isGrounded = MathF.Abs(input.raycastLength - currentLength - input.wheelRadius) < 0.0001f; 

        if ((currentLength + input.wheelRadius) > input.raycastLength || isGrounded)
        {
            currentLength = input.raycastLength - input.wheelRadius;
        }
        
        currentVelocity = (currentLength - lastFrameLength) / dt;
        float acceleration = (currentVelocity - lastFrameVelocity) / dt;

        springForce = (config.RestLength - currentLength) * config.SpringRate;
        damperForce = -(currentVelocity * config.DamperRate);

        if (isGrounded)
        {
            verticalLoad = (config.WheelMass * acceleration) + input.verticalLoad + damperForce + springForce;   
        }
        else
        {
            verticalLoad = (config.WheelMass * gravity) + springForce + damperForce;

            verticalWheelVelocity = verticalWheelVelocity + ((gravity + ((springForce + damperForce)/config.WheelMass)) * dt);
            currentLength += verticalWheelVelocity * dt;
        }

        lastFrameLength = currentLength;
        lastFrameVelocity = currentVelocity;

        SuspensionOutputData output = new();
        output.isGrounded = isGrounded;
        output.verticalLoad = verticalLoad;
        output.suspensionForce = isGrounded ? springForce + damperForce : 0f;
        output.verticalWheelDisplacement = currentLength;
        output.springRate = config.SpringRate;

        return output;
    }
}

internal struct SuspensionInputData
{
    public float raycastLength;
    public float wheelRadius;
    public float verticalLoad;
}

internal struct SuspensionOutputData
{
    public bool isGrounded;
    public float verticalWheelDisplacement;
    public float suspensionForce;
    public float verticalLoad;
    public float springRate;
}