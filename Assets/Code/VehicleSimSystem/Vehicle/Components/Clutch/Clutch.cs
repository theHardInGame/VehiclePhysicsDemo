internal sealed class Clutch : BaseVehicleComponent<ClutchConfig>, IDrivetrainComponent
{
    public Clutch(ClutchConfig config, ISimulationContext simContext) : base(config, simContext)
    {
    }

    BackwardState IDrivetrainComponent.Backward(BackwardState input, float tick)
    {
        return input;
    }

    ForwardState IDrivetrainComponent.Forward(ForwardState input, float tick)
    {
        return input;
    }
}