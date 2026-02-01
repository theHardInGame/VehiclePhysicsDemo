using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class VehicleController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour vehicleInputProvider;
    [SerializeField] private VehicleConfig vehicleConfig;
    [SerializeField] private WheelGO[] wheelGOs;


    private IVehicleInputProvider vehicleIP;
    private Vehicle vehicle;
    private VehicleCommands vehicleCommands;
    private VehicleIOState vehicleIOState;

    private Dictionary<Guid, WheelInputState> wheelInputs;
    private Dictionary<Guid, WheelOutputState> wheelOutputs;

    private void Awake()
    {
        if (vehicleInputProvider is IVehicleInputProvider vip)
        {
            vehicleIP = vip;
        }
        else
        {
            Debug.LogError($"{vehicleInputProvider.name} does not inherit IVehicleInputProvider!");
        }

        CreateWheelIODictionaries();

        vehicleCommands = new();
        vehicleIOState = new VehicleIOState(wheelInputs, wheelOutputs, vehicleCommands);
    }

    private void FixedUpdate()
    {
        SetVehicleCommands();
    }

    private void CreateWheelIODictionaries()
    {
        if (wheelGOs.Length != vehicleConfig.Wheels.Length)
        {
            Debug.LogError("The amount of WheelGO and Wheels in VehicleConfig does not match.");
            return;
        }

        wheelInputs = new();
        wheelOutputs = new();

        for (int i = 0; i < wheelGOs.Length; i++)
        {
            wheelInputs.Add(wheelGOs[i].ID, new WheelInputState());
            wheelOutputs.Add(wheelGOs[i].ID, new WheelOutputState());
        }
    }

    private void SetVehicleCommands()
    {
        vehicleCommands.SetThrottle(vehicleIP.Throttle);
        vehicleCommands.SetBrake(vehicleIP.Brake);
        vehicleCommands.SetSteering(vehicleIP.Steer);
    }
}