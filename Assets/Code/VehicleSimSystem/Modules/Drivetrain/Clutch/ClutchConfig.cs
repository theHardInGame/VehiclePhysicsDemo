using UnityEngine;

[CreateAssetMenu(fileName = "ClutchConfig", menuName = "Vehicle Modules/Drivetrain/Clutch")]
public sealed class ClutchConfig : BaseModuleConfig
{
    [Min(0f)]
    public float reengageRPMMargin;
}