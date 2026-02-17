using System;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public sealed class WheelConfig : BaseComponentConfig
{
    [Tooltip("For debugging and user use.")]
    public string Name;

    [Tooltip("Copy and paste on respective SuspensionJointMB's and WheelMB's ID fields.")]
    [ReadOnly]
    public string ID;

    [Tooltip("Is this wheel powered by the engine?")]
    public bool Powered;

    [Tooltip("Is this wheel steerable?")]
    public bool Steered;

    [Tooltip("Does this wheel recieve brake?")]
    public bool RecieveBrake;

    [Tooltip("Radius of wheel in m")]
    [Min(0)]
    public float Radius;

    [Tooltip("Longitudinal slip to coefficienty of friction curve. (x: 0-1 | y: 0-1)")]
    public AnimationCurve LongSlipCurve;

    [Tooltip("Lateral slip to coefficient of friction curve. (x: 0-1 | y: 0-1)")]
    public AnimationCurve LatSlipCurve;

    [Tooltip("Coefficient of rolling resistance of wheel.")]
    [Min(0)]
    public float RollingResistance;

    [Tooltip("Rotational inertia of wheel.")]
    [Min(0)]
    public float Inertia;

    public SuspensionConfig Suspension;
}