internal interface IModulePort
{
    int GetWheelCount();
    float GetFeedbackTorque(int ID);
    float GetFeedbackRPM(int ID);

    void SetDrivetrainTorque(int ID, float torque);
    void SetDrivetrainRPM(int ID, float rpm);

    void SetSuspensionNormalLoad(int ID, float normalLoad);
}