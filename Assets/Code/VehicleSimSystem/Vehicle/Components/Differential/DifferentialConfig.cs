using UnityEngine;

[System.Serializable]
public sealed class DifferentialConfig : BaseComponentConfig
{
    [Range(0, 1)]
    public float slipCouplingCoefficient;
}