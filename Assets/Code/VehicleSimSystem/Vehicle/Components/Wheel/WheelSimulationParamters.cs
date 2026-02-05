using System.Numerics;

internal sealed class WheelSimulationParamters
{
    public WheelSimulationParamters(bool isPowered, bool isSteered, float radius)
    {
        this.isPowered = isPowered;
        this.isSteered = isSteered;
        this.radius = radius;
    }
    public bool isPowered;
    public bool isSteered;
    public float radius;


    public float feedbackTorque;
    public float feedbackRPM;
    public Vector3 localPos;
    public float raycastLength;
    public Vector3 raycastNormal;
    public Vector3 raycastTangent;


    public float suspensionNormalLoad;
    public float suspensionForce;
    public float verticalWheelDisplacement;
    public float drivetrainTorque;
    public float drivetrainRPM;
}