using UnityEngine;

[System.Serializable]
public sealed class WheelConfig : BaseComponentConfig
{
    [Tooltip("For debugging and user use.")]
    public string Name;

    [Tooltip("Add WheelGO on Unity Gameobject and copy-paste ID from there.")]
    public string ID;

    [Tooltip("Is this wheel powered by the engine?")]
    public bool Powered;

    [Tooltip("Is this wheel steerable?")]
    public bool Steered;

    [Min(0)]
    public float Radius;

    public AnimationCurve SlipCurve;

    [Min(0)]
    public float RollingResistance;

    [Range(0, 1)]
    public float LongitudinalFriction;

    [Range(0, 1)]
    public float LateralFriciton;
    
    [Min(0)]
    public float CorneringStiffness;

    [Min(0)]
    public float Inertia;

    public SuspensionConfig Suspension;
}