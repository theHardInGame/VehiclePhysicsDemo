using UnityEngine;

[System.Serializable]
public sealed class VehicleCommadConfig
{
    [Min(0)]
    public float MaxThrottleRate;
    
    [Min(0)]
    public float MaxSteerRate;
}