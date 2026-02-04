internal sealed class Clutch : BaseVehicleComponent<ClutchConfig>, IDrivetrainComponent
{
    internal Clutch(ClutchConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
    }

    private bool isEngaged;

    public DrivetrainBackwardState Backward(DrivetrainBackwardState input, float tick)
    {
        float rpm = input.feedbackRPM;

        if (rpm < config.AutoDisengageRPM) isEngaged = false;
        else if (rpm > config.AutoDisengageRPM) isEngaged = true;

        if (!isEngaged)
        {
            input.feedbackTorque = 0f;
        }

        return input;
    }

    public DrivetrainForwardState Forward(DrivetrainForwardState input, float tick)
    {
        if (!isEngaged)
        {
            input.power = 0f;
            input.torque = 0f;
            input.rpm = 0f;
        }

        return input;
    }
}