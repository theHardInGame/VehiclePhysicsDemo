[System.Serializable]
public sealed class SuspensionConfig : BaseComponentConfig
{
    public float RestLength;
    public float MinLength;
    public float MaxLength;
    public float SpringRate;
    public float DamperRate;
    public float WheelMass;
}