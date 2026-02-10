internal interface IDrivetrainComponent
{
    float SimulateForwardTorque(float torqueIn, float dt);

    float SimulateBackwardTorque(float torqueIn, float dt);
    
}

internal struct DrivetrainChainData
{
    public float Torque;
    public float RPM;
}