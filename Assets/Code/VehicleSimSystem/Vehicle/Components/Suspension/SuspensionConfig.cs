using UnityEngine;

[System.Serializable]
public sealed class SuspensionConfig : BaseComponentConfig
{
    [Min(0)]
    public float RestLength;

    [Min(0)]
    public float MinLength;
    
    [Min(0)]
    public float MaxLength;
    
    [Min(0)]
    public float SpringRate;
    
    [Min(0)]
    public float DamperRate;
    
    [Min(0)]
    public float WheelMass;
}