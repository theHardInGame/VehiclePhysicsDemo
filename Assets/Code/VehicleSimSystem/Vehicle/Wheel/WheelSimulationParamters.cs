internal sealed class WheelSimulationParamters
{
    public WheelSimulationParamters(bool isPowered, bool isSteered, float radius)
    {
        IsPowered = isPowered;
        IsSteered = isSteered;
        Radius = radius;
    }

    public bool IsPowered { get; set; }
    public bool IsSteered { get; set; }
    public float Radius { get; set; }

    public float WheelTorque { get; set; } = 0f;
    public float WheelRPM { get; set; } = 0f;
    
    
    public float SuspensionLength { get; set; } = 0f;
    public float SpringRelativeVelocity { get; set; } = 0f;
    public float SuspensionForce { get; set; } = 0f;
    public float DrivetrainTorque { get; set; } = 0f;
}