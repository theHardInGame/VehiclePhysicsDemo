using System;
using System.Collections.Generic;
using UnityEngine;

internal abstract class BaseInputComponent : MonoBehaviour, IInputComponent
{
    [SerializeField] private string componentName;
    [SerializeField] private string iD;
    [SerializeField] protected Rigidbody vehicleRBD;
    protected ISimulationInputPort simIPort;

    public void ControllerAwake(Dictionary<Guid, ISimulationInputPort> simIPorts)
    {
        Guid ID;

        if (!Guid.TryParse(iD, out ID))
        {
            Debug.LogError($"ID: {iD} is not a Guid!");
        }

        if (!simIPorts.TryGetValue(ID, out simIPort))
        {
            Debug.LogError("Could not find ISimulationInputPort for InputComponent");
        }

        OnControllerAwake();
    }

    public void ControllerEarlyUpdate(float dt)
    {
        OnControllerEarlyUpdate(dt);
    }

    public void ControllerLateUpdate(float dt)
    {
        OnControllerLateUpdate(dt);
    }

    /// <summary>
    /// Called by VehicleController every FixedUpdate BEFORE simulation updates.
    /// </summary>
    /// <param name="dt">Time.fixedDeltaTime</param>
    protected abstract void OnControllerEarlyUpdate(float dt);

    /// <summary>
    /// Called by VehicleController every FixedUpdate AFTER simulation updates.
    /// </summary>
    /// <param name="dt">Time.fixedDeltatTime</param>
    protected abstract void OnControllerLateUpdate(float dt);

    /// <summary>
    /// Called by VehicleController at Awake for self initialization.
    /// </summary>
    protected abstract void OnControllerAwake();
}