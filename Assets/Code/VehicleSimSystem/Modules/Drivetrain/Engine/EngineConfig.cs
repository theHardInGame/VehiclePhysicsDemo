using UnityEngine;

[CreateAssetMenu(fileName = "EngineConfig", menuName = "Vehicle Modules/Drivetrain/Engine")]
public sealed class EngineConfig : BaseModuleConfig
{
    [Tooltip("Power output as a function of RPM (0–1) \n X-axis: RPM x 1000 \n Y-axis: Power x 50")]
    public AnimationCurve PowerCurve;

    [Tooltip("Engine idle speed (RPM)")]
    [Min(0)]
    public float idleRPM;

    [Tooltip("Maximum engine speed (RPM)")]
    [Min(0)]
    public float maxRPM;

    [Range(0f, 1f)]
    public float Friction;

    [Range(0f, 1f)]
    public float Inertia;
}