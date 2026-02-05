using System.Numerics;

internal interface IModulePort
{
    int GetWheelCount();
    int[] GetPoweredWheels();
    int[] GetSteeredWheels();

    float GetFeedbackTorque(int ID);
    float GetFeedbackRPM(int ID);
    Vector3 GetLocalPos(int ID);
    Vector3 GetRaycastNormal(int ID);
    Vector3 GetRaycastTangent(int ID);
    float GetRadius(int ID);
    float GetRaycastLength(int ID);

    void SetDrivetrainTorque(int ID, float torque);
    void SetDrivetrainRPM(int ID, float rpm);

    void SetSuspensionNormalForce(int ID, float normalLoad);
    void SetSuspensionForce(int ID, float suspensionForce);
    void SetVerticalWheelDisplacement(int ID, float displacement);
}