using System;
using System.Numerics;

internal sealed class Suspension : BaseVehicleComponent<SuspensionConfig>
{
    public Suspension(SuspensionConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
        currentLength = config.RestLength;

        gravity = vSimCtx.GetGravity();
    }

    public int ID;

    private Vector3 gravity;
    public Vector3 vectorFromCG;

    private float currentLength;
    private float lastFrameLength;
    private float currentVelocity;
    private float lastFrameVelocity;

    private bool isGrounded;
    private float springForce;
    private float damperForce;
    private float normalForce;

    private float verticalWheelVelocity;
    

    public SuspensionOutputData Simulate(SuspensionInputData input, float dt)
    {
        float normalGravity = Vector3.Dot(gravity, input.raycastNormal);

        isGrounded = (currentLength + input.wheelRadius) >= input.raycastLength - 0.002f;

        if ((currentLength + input.wheelRadius) > input.raycastLength || isGrounded)
        {
            currentLength = input.raycastLength - input.wheelRadius;
        }
        
        bool maxCompressed = currentLength <= config.MinLength;
        bool maxDrooped = currentLength >= config.MaxLength;

        currentLength = Math.Clamp(currentLength, config.MinLength, config.MaxLength);

        currentVelocity = maxCompressed || maxDrooped ? 0f : (currentLength - lastFrameLength) / dt;
        float acceleration = (currentVelocity - lastFrameVelocity) / dt;

        springForce = (config.RestLength - currentLength) * config.SpringRate;
        damperForce = -(currentVelocity * config.DamperRate);

        float suspensionForce = springForce + damperForce;

        if (isGrounded)
        {
            if (maxCompressed) 
            {
                Vector3 alpha = vSimCtx.GetAngularAcceleration();
                Vector3 omega = vSimCtx.GetAngularVelocity();
                Vector3 a_cm  = vSimCtx.GetLinearAcceleartion();

                Vector3 a_point =
                    a_cm +
                    Vector3.Cross(alpha, vectorFromCG) +
                    Vector3.Cross(omega, Vector3.Cross(omega, vectorFromCG));

                float imposedNormalAcc = Vector3.Dot(a_point, input.raycastNormal);

                normalForce = MathF.Max(0f, (config.WheelMass * imposedNormalAcc) + input.staticNormalForce);

                suspensionForce = normalForce;

            }
            else
            {
                normalForce = MathF.Max(0f, (config.WheelMass * acceleration) + input.staticNormalForce + damperForce + springForce);
            }
        }
        else
        {
            normalForce = 0;

            if (!maxDrooped)
            {
                float acc = (springForce + damperForce) / config.WheelMass;
                verticalWheelVelocity += acc * dt;
                currentLength += verticalWheelVelocity * dt;   
            }
        }

        lastFrameLength = currentLength;
        lastFrameVelocity = currentVelocity;

        return new SuspensionOutputData
        {
            isGrounded = isGrounded,
            normalForce = normalForce,
            suspensionForce = isGrounded ? suspensionForce : 0f,
            verticalWheelDisplacement = currentLength,
            springRate = config.SpringRate,
            maxCompressed = maxCompressed
        };
    }
}

internal struct SuspensionInputData
{
    public float raycastLength;
    public Vector3 raycastNormal;
    public float wheelRadius;
    public float staticNormalForce;
}

internal struct SuspensionOutputData
{
    public bool isGrounded;
    public bool maxCompressed;
    public float verticalWheelDisplacement;
    public float suspensionForce;
    public float normalForce;
    public float springRate;
}