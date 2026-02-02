using System;

internal interface IWheelPort
{
    int RegisterWheel(Guid ID);

    float GetDrivetrainTorque(int ID);
    float GetDrivetrainRPM(int ID);
    float GetSuspensionNormalLoad(int ID);

    void SetFeedbackTorque(int ID, float feedbackTorque);
    void SetFeedbackRPM(int ID, float feedbackRPM);
}