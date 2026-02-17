using System;
using System.Collections.Generic;

internal interface IInputComponent
{
    /// <summary>
    /// To be called by VehicleController at Awake after self initialization.
    /// </summary>
    /// <param name="simIPorts">ISimulationInputPort dictionary</param>
    public void ControllerAwake(Dictionary<Guid, ISimulationInputPort> simIPorts);

    /// <summary>
    /// To be called by VehicleController every FixedUpdate BEFORE simulation updates.
    /// </summary>
    /// <param name="dt">Time.fixedDeltaTime</param>
    public void ControllerEarlyUpdate(float dt);

    /// <summary>
    /// To be called by VehicleController every FixedUpdate AFTER simulation updates.
    /// </summary>
    /// <param name="dt">Time.fixedDeltaTime</param>
    public void ControllerLateUpdate(float dt);
}