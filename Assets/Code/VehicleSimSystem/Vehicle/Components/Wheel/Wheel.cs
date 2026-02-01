using System;

internal sealed class Wheel : BaseVehicleComponent<WheelConfig>, IDrivetrainComponent
{
    public Wheel(WheelConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
        Guid ID;
        bool b = Guid.TryParse(config.ID, out ID);

        wheelIPS = vIOState.GetWheelInputState(ID);
        wheelOPS = vIOState.GetWheelOutputState(ID);
    }

    WheelInputState wheelIPS;
    WheelOutputState wheelOPS;

    public ForwardState Forward(ForwardState input, float tick)
    {
        return input;    
    }

    public BackwardState Backward(BackwardState input, float tick)
    {
        return input;
    }
}