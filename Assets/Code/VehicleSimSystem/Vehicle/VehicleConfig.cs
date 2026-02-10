using UnityEngine;

[CreateAssetMenu(fileName = "VehicleConfig", menuName = "Vehicle")]
public sealed class VehicleConfig : ScriptableObject
{
    public string Name;
    public string ID;
    public float BodyMass;
    public VehicleCommadConfig Commands;
    public EngineConfig Engine;
    public ClutchConfig Clutch;
    public GearboxConfig Gearbox;
    public DifferentialConfig Differential;
    public WheelConfig[] Wheels;
}