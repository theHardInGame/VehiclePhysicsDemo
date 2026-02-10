using UnityEngine;

[System.Serializable]
public sealed class VehicleCommadConfig
{
    [Min(0)]
    public float ThrottleRate;
    
    [Min(0)]
    public float BrakeRate;

    [Min(0)]
    public float SteerRate;
}