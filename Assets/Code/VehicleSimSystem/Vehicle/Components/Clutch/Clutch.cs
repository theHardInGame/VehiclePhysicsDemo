internal sealed class Clutch : BaseVehicleComponent<ClutchConfig>, IDrivetrainComponent
{
    internal Clutch(ClutchConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
    }

    private bool isEngaged;

    public BackwardState Backward(BackwardState input, float tick)
    {
        float rpm = input.rpm;

        if (rpm < config.AutoDisengageRPM) isEngaged = false;
        else if (rpm > config.AutoDisengageRPM) isEngaged = true;

        if (!isEngaged)
        {
            input.loadTorque = 0f;
        }

        return input;
    }

    public ForwardState Forward(ForwardState input, float tick)
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