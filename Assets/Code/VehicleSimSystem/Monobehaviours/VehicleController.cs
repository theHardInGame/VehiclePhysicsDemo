using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal sealed class VehicleController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour vehicleInputProvider;
    [SerializeField] private VehicleConfig vehicleConfig;
    [SerializeField] private WheelMB[] wheelMBs;
    [SerializeField] private Rigidbody body;
    [SerializeField] private TextMeshProUGUI debugText;


    private IVehicleInputProvider vehicleIP;
    private Vehicle vehicle;
    private VehicleSimulationContext vSimCtx;
    private VehicleIOState vIOS;
    private SimulationPort wheelModulePort;

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
        body.mass = vehicleConfig.BodyMass;

        vIOS = new VehicleIOState(wheelInputs, wheelOutputs, vSimCtx);
        wheelModulePort = new(vehicleConfig.Wheels.Length);

        VehicleBuilder builder = new VehicleBuilder(vIOS, wheelModulePort);
        vehicle = builder.Build(vehicleConfig);
        vehicle.Activate();

        wheelInputs = null;
        wheelOutputs = null;
        //vIOS = null;
    }


    private void FixedUpdate()
    {
        foreach (WheelMB wheel in wheelMBs) wheel.WheelIPSRead();

        UpdateVehicleContext();
        vehicle.FixedUpdate(Time.fixedDeltaTime);

        foreach (WheelMB wheel in wheelMBs) wheel.ControllerUpdate(Time.fixedDeltaTime);
        
        DebugUpdate();
    }

    private void CreateWheelIODictionaries()
    {
        
        if (wheelMBs.Length != vehicleConfig.Wheels.Length)
        {
            Debug.LogError($"The amount of WheelGO and Wheels in VehicleConfig does not match. \n Disabling VehicleController... \n Name: { vehicleConfig.Name } \n ID: { vehicleConfig.ID }", this);
            
            enabled = false;
            return;
        }

        wheelInputs = new();
        wheelOutputs = new();

        for (int i = 0; i < wheelMBs.Length; i++)
        {
            wheelMBs[i].ControllerAwake(vehicleConfig.Wheels[i].Suspension.WheelMass);

            WheelMB wheelMB = wheelMBs[i];
            wheelInputs.Add(wheelMB.ID, wheelMB.WheelIPS);
            wheelOutputs.Add(wheelMB.ID, wheelMB.WheelOPS);
        }
    }

    private float Throttle;
    private float Brake;
    private float Steer;
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
}