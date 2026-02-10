using System;

internal sealed class Gearbox : BaseVehicleComponent<GearboxConfig>, IDrivetrainComponent, IASGearbox
{
    internal Gearbox(GearboxConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
        this.vIOState = null;
    }

    private int currentGear = 0;
    private bool canShift => currentGear > 0 && currentGear < config.GearRatios.Length - 1;
    #region IDrivetrainComponent Interface
    // ===============================================
    // IDrivetrainComponent Interface Implementation
    // ===============================================

    public float SimulateForwardTorque(float torqueIn, float dt)
    {
        if (vSimCtx.DriveshaftRPM > config.UpshiftRPM && vSimCtx.VehicleSpeed > config.UpshitSpeed[currentGear] && canShift) RequestShift?.Invoke(ShiftRequest.Upshift);
        return torqueIn * config.GearRatios[currentGear] * config.FinalDrive;
    }

    public float SimulateBackwardTorque(float torqueIn, float dt)
    {
        if (vSimCtx.DriveshaftRPM < config.DownshiftRPM && canShift) RequestShift?.Invoke(ShiftRequest.Downshift);
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