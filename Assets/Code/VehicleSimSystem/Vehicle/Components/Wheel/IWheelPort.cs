using System;
using System.Numerics;

internal interface IWheelPort
{
    int RegisterWheel(Guid ID, bool isPowered, bool isSteered, float radius);

    float GetDrivetrainTorque(int ID);
    float GetDrivetrainRPM(int ID);
    float GetSuspensionNormalLoad(int ID);

    void SetFeedbackTorque(int ID, float feedbackTorque);
    void SetFeedbackRPM(int ID, float feedbackRPM);
    void SetLocalPos(int ID, Vector3 locaPos);
    void SetGroundHeight(int ID, float height);
}