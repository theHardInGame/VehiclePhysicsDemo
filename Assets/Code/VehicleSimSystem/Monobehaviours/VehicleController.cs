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
    private VehicleSimulationContext vSimCtx;
    private VehicleIOState vIOS;
    private WheelModulePort wheelModulePort;

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

        vSimCtx = new();
        vIOS = new VehicleIOState(wheelInputs, wheelOutputs, vSimCtx);
        wheelModulePort = new(vehicleConfig.Wheels.Length);

        VehicleBuilder builder = new VehicleBuilder(vIOS, wheelModulePort);
        vehicle = builder.Build(vehicleConfig);
    }

    private void FixedUpdate()
    {
        SetVehicleCommands();
        vehicle.FixedUpdate(Time.fixedDeltaTime);
    }

    private void CreateWheelIODictionaries()
    {
        if (wheelGOs.Length != vehicleConfig.Wheels.Length)
        {
            Debug.LogError($"The amount of WheelGO and Wheels in VehicleConfig does not match. \n Disabling VehicleController... \n Name: { vehicleConfig.Name } \n ID: { vehicleConfig.ID }", this);
            
            enabled = false;
            return;
        }

        wheelInputs = new();
        wheelOutputs = new();

        for (int i = 0; i < wheelGOs.Length; i++)
        {
            WheelGO wheelGO = wheelGOs[i];
            wheelInputs.Add(wheelGO.ID, wheelGO.WheelIPS);
            wheelOutputs.Add(wheelGO.ID, wheelGO.WheelOPS);
        }
    }

    private void SetVehicleCommands()
    {
        vSimCtx.SetThrottle(vehicleIP.Throttle);
        vSimCtx.SetBrake(vehicleIP.Brake);
        vSimCtx.SetSteering(vehicleIP.Steer);
    }
}