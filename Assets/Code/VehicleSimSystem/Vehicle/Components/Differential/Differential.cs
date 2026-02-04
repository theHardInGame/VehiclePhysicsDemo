using UnityEngine;

internal sealed class Differetial : BaseVehicleComponent<DifferentialConfig>, IJunctionComponent<DifferentialForwardState, DifferentialBackwardState>
{
    public Differetial(DifferentialConfig config, VehicleIOState vIOState) : base(config, vIOState)
    {
    }

    private float avgFeedbackRPM;
    private float feedbackTorque;

    public DifferentialBackwardState Backward(DifferentialBackwardState feedback, float dt)
    {
        // Calculate Feedback

        float sumRPM = 0f;
        feedbackTorque = 0f;
        int wheelCount = feedback.feedbackRPMs.Length;

        for (int i = 0; i < wheelCount; i++)
        {
            sumRPM += feedback.feedbackRPMs[i];
            feedbackTorque += feedback.feedbackTorques[i];
        }

        avgFeedbackRPM = sumRPM / wheelCount;
        feedbackTorque = Mathf.Clamp(feedbackTorque, -config.MaxDifferentialTorque, config.MaxDifferentialTorque);

        // Assigning feedback

        feedback.backward.feedbackTorque = feedbackTorque;
        feedback.backward.feedbackRPM = avgFeedbackRPM;

        return feedback;
    }

    public DifferentialForwardState Forward(DifferentialForwardState input, float dt)
    {
        int wheelCount = input.wheelRPMs.Length;

        if (Mathf.Approximately(avgFeedbackRPM, 0f))
        {
            float equalTorque = input.forward.torque / wheelCount;

            for (int i = 0; i < wheelCount; i++)
            {
                input.wheelTorques[i] = equalTorque;
                input.wheelRPMs[i] = input.forward.rpm;
            }
        }
        else
        {
            // LSD Differential
            input = ApplyLSD(input);
        }

        return input;
    }

    private DifferentialForwardState ApplyLSD(DifferentialForwardState input)
    {
        int wheelCount = input.wheelTorques.Length;
        float totalTorque = input.forward.torque;

        float[] torques = new float[wheelCount];
        for (int i = 0; i < wheelCount; i++)
        {
            // torque bias based on wheel RPM difference
            float rpmDiff = Mathf.Abs(input.wheelRPMs[i] - avgFeedbackRPM);
            float lockFactor = Mathf.Min(rpmDiff / config.LSDSlipRPM, 1f); // 0..1
            lockFactor = Mathf.Pow(lockFactor, config.LSDResponseCurve); // soften
            float bias = Mathf.Lerp(1f, config.LSDTorqueBias, lockFactor);

            torques[i] = totalTorque * bias / wheelCount; // naive scaling
        }

        // Normalize torque to match total torque
        float sumTorques = 0f;
        for (int i = 0; i < wheelCount; i++) sumTorques += torques[i];
        for (int i = 0; i < wheelCount; i++) torques[i] *= totalTorque / sumTorques;

        input.wheelTorques = torques;

        // All wheels get shaft RPM
        for (int i = 0; i < wheelCount; i++)
            input.wheelRPMs[i] = input.forward.rpm;

        return input;
    }
}

internal struct DifferentialForwardState
{
    public DrivetrainForwardState forward;
    public float[] wheelTorques;
    public float[] wheelRPMs;
}

internal struct DifferentialBackwardState
{
    public DrivetrainBackwardState backward;
    public float[] feedbackTorques;
    public float[] feedbackRPMs;
}