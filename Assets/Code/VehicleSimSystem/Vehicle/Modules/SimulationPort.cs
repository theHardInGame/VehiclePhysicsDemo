using System;
using System.Collections.Generic;

internal sealed class SimulationPort : IModulePort, IWheelPort
{
    private WheelSimulationParamters[] wheels;
    private List<WheelSimulationParamters> wheelList = new();

    private Dictionary<Guid, int> wheelsGuidToIndex = new();
    private int nextIndex = 0;

    private readonly int wheelCount;
    private readonly List<int> poweredWheels = new();
    private readonly List<int> steeredWheels = new();

    public SimulationPort(int wheelCount) => this.wheelCount = wheelCount;

    #region IWheelPort
    public int RegisterWheel(Guid ID, bool isPowered, bool isSteered, float radius)
    {
        if (wheelsGuidToIndex.ContainsKey(ID)) throw new Exception("Duplicate wheel registration in SimulationPort");

        int index = nextIndex++;

        wheelsGuidToIndex.Add(ID, index);
        wheelList.Add(new(isPowered, isSteered, radius));

        if (isPowered) poweredWheels.Add(index);
        if (isSteered) steeredWheels.Add(index);

        if (index + 1 == wheelCount)
        {
            wheels = wheelList.ToArray();
            wheelList = null;
            wheelsGuidToIndex = null;
        }

        return index;
    }

    public float GetDrivetrainTorque(int ID) => wheels[ID].DrivetrainTorque;
    public float GetSuspensionForce(int ID) => wheels[ID].SuspensionForce;

    public void SetWheelTorque(int ID, float feedbackTorque) => wheels[ID].WheelTorque = feedbackTorque;
    public void SetWheelRPM(int ID, float feedbackOmega) => wheels[ID].WheelRPM = feedbackOmega;
    public void SetSpringLength(int ID, float height) => wheels[ID].SuspensionLength = height;
    public void SetSpringRelativeVelocity(int ID, float relativeVelocity) => wheels[ID].SpringRelativeVelocity = relativeVelocity;
    #endregion


    #region IModulePort
    public int GetWheelCount() => wheelCount;
    public int[] GetPoweredWheels() => poweredWheels.ToArray();
    public int[] GetSteeredWheels() => steeredWheels.ToArray();
    public float GetWheelTorque(int ID) => wheels[ID].WheelTorque;
    public float GetWheelRPM(int ID) => wheels[ID].WheelRPM;
    public float GetSuspensionLength(int ID) => wheels[ID].SuspensionLength;
    public float GetSpringRelativeVelocity(int ID) => wheels[ID].SpringRelativeVelocity;

    public void SetDrivetrainTorque(int ID, float torque) => wheels[ID].DrivetrainTorque = torque;
    public void SetSuspensionForce(int ID, float force) => wheels[ID].SuspensionForce = force;
    #endregion
}