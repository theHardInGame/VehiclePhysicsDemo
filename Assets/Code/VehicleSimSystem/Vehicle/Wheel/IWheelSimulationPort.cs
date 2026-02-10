using System;

internal interface IWheelSimulationPort
{
    int RegisterWheel(Guid ID, bool isPowered, bool isSteered, float radius);

    float GetDrivetrainTorque(int ID);
    float GetSuspensionForce(int ID);

    void SetWheelTorque(int ID, float wheelTorque);
    void SetWheelRPM(int ID, float wheelRPM);
    void SetSpringLength(int ID, float height);
    void SetSpringRelativeVelocity(int ID, float relativeVelocity);
}