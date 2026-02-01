internal sealed class Differetial : BaseVehicleComponent<DifferentialConfig>, IDrivetrainComponent
{
    public Differetial(DifferentialConfig config, VehicleIOState vIOState) : base(config, vIOState)
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