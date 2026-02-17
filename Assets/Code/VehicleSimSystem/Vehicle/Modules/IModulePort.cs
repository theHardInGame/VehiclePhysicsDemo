internal interface IModulePort
{
    int GetWheelCount();
    int[] GetPoweredWheels();
    int[] GetSteeredWheels();

    float GetWheelTorque(int ID);
    float GetWheelRPM(int ID);
    float GetSuspensionLength(int ID);
    float GetSpringRelativeVelocity(int ID);

    void SetDrivetrainTorque(int ID, float torque);
    void SetSuspensionForce(int ID, float suspensionForce);
}