using UnityEngine;

[System.Serializable]
public sealed class EngineConfig : BaseComponentConfig
{
    [Tooltip("RPM - N.m Curve")]
    public AnimationCurve RPMTorqueCurve;

    [Tooltip("Idle RPM of the engine.")]
    [Min(0)]
    public float IdleRPM;

    [Tooltip("Maximum RPM of the engine.")]
    [Min(0)]
    public float MaxRPM;

    [Tooltip("Internal frictional drag of the engine. High value means higher frictional losses.")]
    [Min(0)]
    public float EngineDrag;

    [Tooltip("Rate of RPM recovery when RPM is less than Idle RPM and clutch is disengaged")]
    [Min(0)]
    public float IdleRecoveryStrength;

    [Tooltip("Kg.m^2")]
    [Min(0)]
    public float RotationalInertia;

}