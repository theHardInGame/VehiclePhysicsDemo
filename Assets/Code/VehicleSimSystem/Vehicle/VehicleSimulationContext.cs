using System;
using System.Collections.Generic;

internal sealed class VehicleSimulationContext
{
    #region Input States
    // ==============
    // Input States
    // ==============
    
    internal float Throttle { get; private set; }
    internal float Brake { get; private set; }
    internal float Steering { get; private set; }

    internal void SetThrottle(float throttle) { this.Throttle = throttle; }
    internal void SetBrake(float brake) { this.Brake = brake; }
    internal void SetSteering(float steering) { this.Steering = steering; }
    #endregion


    #region Simulated States
    // ==================
    // Simulated States
    // ==================
    internal float EngineRPM { get; private set;}
    internal void SetEngineRPM(float rpm) { EngineRPM = rpm; }

    internal float DriveshaftRPM { get; private set; }
    internal void SetDriveshaftRPM(float RPM) { DriveshaftRPM = RPM; }

    internal int CurrentGear { get; private set; }
    internal void SetCurrentGear(int index) => CurrentGear = index;

    internal float VehicleSpeed { get; private set; }
    internal void SetVehicleSpeed(float speed) => VehicleSpeed = speed;
    #endregion

    internal Dictionary<Guid, ISimulationOuputPort> simOPorts;
}