using UnityEngine;

//[CreateAssetMenu(fileName = "EngineConfig", menuName = "Vehicle Modules/Drivetrain/Engine")]
[System.Serializable]
public sealed class EngineConfig : BaseComponentConfig
{
    [Tooltip("RPM - N.m Curve")]
    public AnimationCurve RPMTorqueCurve;

    [Min(0)]
    public float IdleRPM;

    [Min(0)]
    public float MaxRPM;

    [Min(0)]
    public float EngineDrag;

    [Min(0)]
    public float IdleRecoveryStrength;

    [Tooltip("Kg.m^2")]
    [Min(0)]
    public float RotationalInertia;

}