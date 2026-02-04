using UnityEngine;

[System.Serializable]
public sealed class DifferentialConfig : BaseComponentConfig
{
    [Min(0f)]
    public float MaxDifferentialTorque;
    
    [Min(0f)]
    public float LSDSlipRPM;
    
    [Min(0f)]
    public float LSDResponseCurve;

    [Min(1f)]
    public float LSDTorqueBias;
}