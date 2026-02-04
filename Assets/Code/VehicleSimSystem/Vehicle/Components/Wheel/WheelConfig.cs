using UnityEngine;

[System.Serializable]
public sealed class WheelConfig : BaseComponentConfig
{
    [Tooltip("For debugging and user use.")]
    public string Name;

    [Tooltip("Add WheelGO on Unity Gameobject and copy-paste ID from there.")]
    public string ID;

    [Tooltip("Is this wheel powered by the engine?")]
    public bool Powered;

    [Tooltip("Is this wheel steerable?")]
    public bool Steered;

    public float Radius;

    public SuspensionConfig Suspension;
}