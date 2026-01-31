internal sealed class Wheel : BaseVehicleComponent<WheelConfig>, IDrivetrainComponent
{
    public Wheel(WheelConfig config, ISimulationContext simContext) : base(config, simContext)
    {
    }

    public ForwardState Forward(ForwardState input, float tick)
    {
        return input;    
    }

    public BackwardState Backward(BackwardState input, float tick)
    {
        return input;
    }
}