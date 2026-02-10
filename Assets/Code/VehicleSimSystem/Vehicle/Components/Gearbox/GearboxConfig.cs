using UnityEngine;

[System.Serializable]
public sealed class GearboxConfig : BaseComponentConfig
{
    public float[] GearRatios;
    public float[] UpshitSpeed;

    [Min(0)]
    public float FinalDrive;

    [Min(0)]
    public float UpshiftRPM;

    [Min(0)]
    public float DownshiftRPM;
}