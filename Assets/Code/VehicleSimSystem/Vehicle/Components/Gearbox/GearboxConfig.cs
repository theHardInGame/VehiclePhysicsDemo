using UnityEditor.EditorTools;
using UnityEngine;

[System.Serializable]
public sealed class GearboxConfig : BaseComponentConfig
{
    [Tooltip("Gear ratios in OutputToruqe / InputTorque")]
    public float[] GearRatios;

    [Tooltip("(currently unimplemented)")]
    public float[] UpshitSpeed;

    [Min(0)]
    public float FinalDrive;

    [Tooltip("(currently unimplemented)")]
    [Min(0)]
    public float UpshiftRPM;

    [Min(0)]
    public float DownshiftRPM;
}