internal interface IDrivetrainComponent
{
    DrivetrainForwardState Forward(DrivetrainForwardState input, float tick);
    DrivetrainBackwardState Backward(DrivetrainBackwardState input, float tick);
}

internal struct DrivetrainForwardState
{
    internal float power;
    internal float rpm;
    internal float torque;
}

internal struct DrivetrainBackwardState
{
    internal float feedbackTorque;
    internal float feedbackRPM;
}