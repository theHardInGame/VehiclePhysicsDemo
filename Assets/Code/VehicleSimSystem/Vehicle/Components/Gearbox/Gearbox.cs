using UnityEngine;

internal sealed class Gearbox : BaseVehicleComponent<GearboxConfig>, IDrivetrainComponent
{
    internal Gearbox(GearboxConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
    }

    public BackwardState Backward(BackwardState input, float tick)
    {
        float power = 2 * input.rpm * Mathf.PI * input.loadTorque / 60;
        input.rpm *= config.GearRatio;
        input.loadTorque = power * 60 / (input.rpm  * 2 * Mathf.PI);

        return input;
    }

    public ForwardState Forward(ForwardState input, float tick)
    {
        input.rpm /= config.GearRatio;
        input.torque = input.power * 60 / (input.rpm * 2 * Mathf.PI);

        return input;
    }
}