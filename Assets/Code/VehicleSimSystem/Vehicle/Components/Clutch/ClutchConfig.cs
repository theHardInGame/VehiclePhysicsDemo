using UnityEngine;

[System.Serializable]
public sealed class ClutchConfig : BaseComponentConfig
{
    [Tooltip("Total time of Clutch Disengage -> Gear Shift -> Clutch Reengage (in seconds)")]
    [Min(0)]
    public float AutoShiftTime;

    [Tooltip("RPM of engine at which clutch disengages to protect from stall")]
    [Min(0)]
    public float AutoDisengageRPM;
}