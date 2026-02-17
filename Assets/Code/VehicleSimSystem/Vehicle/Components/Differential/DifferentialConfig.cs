using UnityEngine;

[System.Serializable]
public sealed class DifferentialConfig : BaseComponentConfig
{
    [Tooltip("Slipping ratio for Limited Slip Differential. \n1 - Locked differential. \n0 - Open differential.")]
    [Range(0, 1)]
    public float slipCouplingCoefficient;
}