public sealed class Clutch : BaseVehicleModule<ClutchConfig>, IDrivetrainModule
{
    public Clutch(ClutchConfig config) : base(config)
    {
    }

    BackwardState IDrivetrainModule.Backward(BackwardState input)
    {
        return input;
    }

    ForwardState IDrivetrainModule.Forward(ForwardState input)
    {
        return input;
    }
}