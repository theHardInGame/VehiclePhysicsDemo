using System;

internal sealed class Wheel : BaseVehicleComponent<WheelConfig>
{
    public Wheel(WheelConfig config, VehicleIOState vIOState, IWheelPort wheelPort) : base(config, vIOState)
    {
        if (!Guid.TryParse(config.ID, out Guid id))
        {
            throw new InvalidOperationException($"Invalid WheelConfig ID '{config.ID}' in {config.Name}");
        }
        
        wheelIPS = vIOState.GetWheelInputState(id);
        wheelOPS = vIOState.GetWheelOutputState(id);

        this.wheelPort = wheelPort;

        wheelPort.RegisterWheel(id, config.Powered, config.Steered, config.Radius);
    }

    WheelInputState wheelIPS;
    WheelOutputState wheelOPS;
    IWheelPort wheelPort;

    #region Drivetrain Interface Implementation
    // =====================================
    // Drivetrain Interface Implementation
    // =====================================

    public DrivetrainForwardState Forward(DrivetrainForwardState input, float tick)
    {
        return input;
    }

    public DrivetrainBackwardState Backward(DrivetrainBackwardState input, float tick)
    {
        return input;
    }
    #endregion
}