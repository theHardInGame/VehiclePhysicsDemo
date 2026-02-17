using System;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleConfig", menuName = "Vehicle")]
public sealed class VehicleConfig : ScriptableObject
{
    [Tooltip("Name of the vehicle. For Debugging.")]
    public string Name;

    [Tooltip("Unique ID for vehicle. Currently unused. Can be left empty.")]
    public string ID;

    [Tooltip("Total mass of vehicle body (not including wheels etc) in Kg")]
    public float BodyMass;

    public VehicleCommadConfig Commands;
    public EngineConfig Engine;
    public ClutchConfig Clutch;
    public GearboxConfig Gearbox;
    public DifferentialConfig Differential;
    public WheelConfig[] Wheels;


#if UNITY_EDITOR
    private void OnValidate()
    {
        for (int i = 0; i < Wheels.Length; i++)
        {
            WheelConfig wheel = Wheels[i];

            if (string.IsNullOrEmpty(wheel.ID))
            {
                wheel.ID = Guid.NewGuid().ToString();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
        
    }
#endif
}