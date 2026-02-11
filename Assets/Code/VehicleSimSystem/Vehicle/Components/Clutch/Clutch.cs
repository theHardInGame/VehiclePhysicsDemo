internal sealed class Clutch : BaseVehicleComponent<ClutchConfig>, IDrivetrainComponent, IASClutch
{
    internal Clutch(ClutchConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
        this.vIOState = null;
        engagementRatio = 0f;
    }

    private float engagementRatio; // 0 = fully slip, 1 = locked

    private float engineSideTorque;
    private float wheelSideTorque;


    #region IDrivetrainComponent Interface
    // ===============================================
    // IDrivetrainComponent Interface Implementation
    // ===============================================

    public float SimulateForwardTorque(float torqueIn, float dt)
    {
        engineSideTorque = torqueIn;
        StallSafety();
        return engineSideTorque * engagementRatio;
    }

    public float SimulateBackwardTorque(float torqueIn, float dt)
    {
        wheelSideTorque = torqueIn;
        StallSafety();
        return wheelSideTorque * engagementRatio;
    }
    #endregion


    #region IASClutch Interface
    // ====================================
    // IASClutch Interface Implementation
    // ====================================
    
    private bool isShifting = false;
    private float shiftTimer = 0;
    public void Tick(float dt)
    {
        if (!isShifting)
        {
            StallSafety();
            return;
        }

        if (shiftTimer <= 0f)
        {
            engagementRatio = 1;
            isShifting = false;
            return;
        }

        shiftTimer -= dt;
    }

    public void StallSafety()
    {
        if (vSimCtx.EngineRPM < config.AutoDisengageRPM) engagementRatio = 0;
        else engagementRatio = 1f;
    }

    public void StartAutoShift()
    {
        engagementRatio = 0;
        shiftTimer = config.AutoShiftTime;
        isShifting = true;
    }
    #endregion
}