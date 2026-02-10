using System;

internal sealed class Gearbox : BaseVehicleComponent<GearboxConfig>, IDrivetrainComponent, IASGearbox
{
    internal Gearbox(GearboxConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
        this.vIOState = null;
    }

    private int currentGear = 0;
    private bool canShiftUp => currentGear < config.GearRatios.Length - 1;
    private bool canShiftDown => currentGear > 0;
    #region IDrivetrainComponent Interface
    // ===============================================
    // IDrivetrainComponent Interface Implementation
    // ===============================================

    public float SimulateForwardTorque(float torqueIn, float dt)
    {
        if (vSimCtx.EngineRPM > config.UpshiftRPM && vSimCtx.VehicleSpeed > config.UpshitSpeed[currentGear] && canShiftUp) RequestShift?.Invoke(ShiftRequest.Upshift);
        return torqueIn * config.GearRatios[currentGear] * config.FinalDrive;
    }

    public float SimulateBackwardTorque(float torqueIn, float dt)
    {
        if (vSimCtx.DriveshaftRPM * config.GearRatios[currentGear] * config.FinalDrive < config.DownshiftRPM && canShiftDown) RequestShift?.Invoke(ShiftRequest.Downshift);
        return torqueIn / (config.GearRatios[currentGear] * config.FinalDrive);
    }
    #endregion

    #region IASGearbox Interface
    // =====================================
    // IASGearbox Interface Implementation
    // =====================================
    public event Action<ShiftRequest> RequestShift;

    public void Upshift()
    {
        currentGear++;
        RequestShift?.Invoke(ShiftRequest.None);
        vSimCtx.SetCurrentGear(currentGear);
    }

    public void Downshift()
    {
        currentGear--;
        RequestShift?.Invoke(ShiftRequest.None);
        vSimCtx.SetCurrentGear(currentGear);
    }
    #endregion
}