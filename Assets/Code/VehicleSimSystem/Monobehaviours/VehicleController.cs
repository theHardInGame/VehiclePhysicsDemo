using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal sealed class VehicleController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour vehicleInputProvider;
    [SerializeField] private VehicleConfig vehicleConfig;
    [SerializeField] private MonoBehaviour[] iInputComponents;
    [SerializeField] private Rigidbody body;
    [SerializeField] private TextMeshProUGUI debugText;

    private IInputComponent[] inputComponents;
    private IVehicleInputProvider vehicleIP;
    private Vehicle vehicle;
    private VehicleSimulationContext vSimCtx;
    private SimulationPort wheelModulePort;

    private Dictionary<Guid, ISimulationOuputPort> simOPorts;
    private Dictionary<Guid, ISimulationInputPort> simIPorts;


    #region Unity API
    // ===========
    // Unity API
    // ===========

    private void Awake()
    {
        SetupInputs();
        InitializeDependencies();
        BuildVehicle();

        for (int i = 0; i < inputComponents.Length; i++)
        {
            inputComponents[i].ControllerAwake(simIPorts);
        }

        simOPorts = null;
        simIPorts = null;
    }

    private void FixedUpdate()
    {
        foreach (IInputComponent component in inputComponents) component.ControllerEarlyUpdate(Time.fixedDeltaTime);

        UpdateVehicleContext();
        vehicle.FixedUpdate(Time.fixedDeltaTime);

        foreach (IInputComponent component in inputComponents) component.ControllerLateUpdate(Time.fixedDeltaTime);
        
        DebugUpdate();
    }
    #endregion

    #region Controller Initializations
    // ============================
    // Controller Initializations
    // ============================

    /// <summary>
    /// Sets up VehcileInputProvider
    /// </summary>
    private void SetupInputs()
    {
        if (vehicleInputProvider is IVehicleInputProvider vip)
        {
            vehicleIP = vip;
        }
        else
        {
            Debug.LogError($"{vehicleInputProvider.name} does not inherit IVehicleInputProvider!");
        }
    }

    /// <summary>
    /// Initializes dependencies for Vehicle
    /// </summary>
    private void InitializeDependencies()
    {
        inputComponents = new IInputComponent[iInputComponents.Length];
        for (int i = 0; i < iInputComponents.Length; i++)
        {
            if (iInputComponents[i] is IInputComponent component) inputComponents[i] = component;
            else Debug.LogError("Monobehaviour does not implement IOutputComponenet");
        }
        
        CreateWheelIODictionaries();

        vSimCtx = new()
        {
            simOPorts = simOPorts
        };

        body.mass = vehicleConfig.BodyMass;
        wheelModulePort = new(vehicleConfig.Wheels.Length);
    }

    /// <summary>
    /// Helper for Dependency Initialization
    /// Creates SimulationIOParameter Dictionaries
    /// </summary>
    private void CreateWheelIODictionaries()
    {
        if (inputComponents.Length / 2 != vehicleConfig.Wheels.Length)
        {
            Debug.LogError($"The amount of WheelGO and Wheels in VehicleConfig does not match. \n Disabling VehicleController... \n Name: { vehicleConfig.Name } \n ID: { vehicleConfig.ID }", this);
            
            enabled = false;
            return;
        }

        simOPorts = new();
        simIPorts = new();

        for (int i = 0; i < vehicleConfig.Wheels.Length; i++)
        {
            WheelConfig wheel = vehicleConfig.Wheels[i];
            Guid ID = Guid.Parse(wheel.ID);
            SimulationIOParamters simIOPort = new(wheel.Suspension.WheelMass);
            simOPorts.Add(ID, simIOPort);
            simIPorts.Add(ID, simIOPort);
        }
    }

    /// <summary>
    /// Creates and Builds Vehicle
    /// </summary>
    private void BuildVehicle()
    {
        VehicleBuilder builder = new VehicleBuilder(vSimCtx, wheelModulePort);
        vehicle = builder.Build(vehicleConfig);
        vehicle.Activate();
    }
    #endregion

    #region Controller Updates
    // ====================
    // Controller Updates
    // ====================

    private float Throttle;
    private float Brake;
    private float Steer;

    /// <summary>
    /// Reads and updates player inputs
    /// </summary>
    private void UpdateVehicleContext()
    {
        float throttleRate = vehicleConfig.Commands.ThrottleRate * Time.fixedDeltaTime;
        Throttle = Mathf.MoveTowards(Throttle, vehicleIP.Throttle, throttleRate);

        float brakeRate = vehicleConfig.Commands.BrakeRate * Time.fixedDeltaTime;
        Brake = Mathf.MoveTowards(Brake, vehicleIP.Brake, brakeRate);

        float steerRate = vehicleConfig.Commands.SteerRate * Time.fixedDeltaTime;
        Steer = Mathf.MoveTowards(Steer, vehicleIP.Steer, steerRate);

        vSimCtx.SetThrottle(Throttle);
        vSimCtx.SetBrake(Brake);
        vSimCtx.SetSteering(Steer);

        vSimCtx.SetVehicleSpeed(body.transform.InverseTransformDirection(body.linearVelocity).x);
    }

    /// <summary>
    /// Debug
    /// </summary>
    private void DebugUpdate()
    {
        debugText.text = $"Throttle: { vSimCtx.Throttle }\n";
        debugText.text += $"Brake: { vSimCtx.Brake }\n";
        debugText.text += $"Steering: { vSimCtx.Steering }\n";
        debugText.text += $"Vehicle Speed: { vSimCtx.VehicleSpeed }\n";
        debugText.text += $"Engine RPM: { vSimCtx.EngineRPM }\n";
        debugText.text += $"Driveshaft RPM: { vSimCtx.DriveshaftRPM }\n";
        debugText.text += $"Current Gear: { vSimCtx.CurrentGear + 1 }\n";
    }
    #endregion
}