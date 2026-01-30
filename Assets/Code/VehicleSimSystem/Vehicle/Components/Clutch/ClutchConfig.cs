using UnityEngine;

//[CreateAssetMenu(fileName = "ClutchConfig", menuName = "Vehicle Modules/Drivetrain/Clutch")]
[System.Serializable]
public sealed class ClutchConfig : BaseComponentConfig
{
    [Min(0f)]
    public float reengageRPMMargin;
}