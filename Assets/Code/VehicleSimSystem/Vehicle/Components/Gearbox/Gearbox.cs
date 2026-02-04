using UnityEngine;

internal sealed class Gearbox : BaseVehicleComponent<GearboxConfig>, IDrivetrainComponent
{
    internal Gearbox(GearboxConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
    }

    public DrivetrainBackwardState Backward(DrivetrainBackwardState input, float tick)
    {
        float power = 2 * input.feedbackRPM * Mathf.PI * input.feedbackTorque / 60;
        input.feedbackRPM *= config.GearRatio;
        input.feedbackTorque = power * 60 / (input.feedbackRPM  * 2 * Mathf.PI);

        return input;
    }

    public DrivetrainForwardState Forward(DrivetrainForwardState input, float tick)
    {
        input.rpm /= config.GearRatio;
        input.torque = input.power * 60 / (input.rpm * 2 * Mathf.PI);

        return input;
    }
}