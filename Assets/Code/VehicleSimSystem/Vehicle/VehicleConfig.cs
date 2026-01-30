using UnityEngine;

[CreateAssetMenu(fileName = "VehicleConfig", menuName = "Vehicle")]
public sealed class VehicleConfig : ScriptableObject
{
    public EngineConfig Engine;
    public ClutchConfig Clutch;
}