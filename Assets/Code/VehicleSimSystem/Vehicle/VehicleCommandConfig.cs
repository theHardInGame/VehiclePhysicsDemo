using UnityEditor.EditorTools;
using UnityEngine;

[System.Serializable]
public sealed class VehicleCommadConfig
{
    [Tooltip("Rate at which Throttle saturates.")]
    [Min(0)]
    public float ThrottleRate;
    
    [Tooltip("Rate at which Brake saturates.")]
    [Min(0)]
    public float BrakeRate;

    [Tooltip("Rate at which Steering saturates.")]
    [Min(0)]
    public float SteerRate;
}