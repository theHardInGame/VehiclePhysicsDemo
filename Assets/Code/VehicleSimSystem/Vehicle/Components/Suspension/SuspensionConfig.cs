using UnityEditor.EditorTools;
using UnityEngine;

[System.Serializable]
public sealed class SuspensionConfig : BaseComponentConfig
{
    [Tooltip("Rest length of suspension spring.")]
    [Min(0)]
    public float RestLength;

    [Tooltip("Minimum length of suspension.")]
    [Min(0)]
    public float MinLength;
    
    [Tooltip("Maximum length of suspension.")]
    [Min(0)]
    public float MaxLength;
    
    [Tooltip("Spring strength in N/m")]
    [Min(0)]
    public float SpringRate;
    
    [Tooltip("Damper strength in N * s/m")]
    [Min(0)]
    public float DamperRate;
    
    [Tooltip("Mass of wheel in Kg")]
    [Min(0)]
    public float WheelMass;
}