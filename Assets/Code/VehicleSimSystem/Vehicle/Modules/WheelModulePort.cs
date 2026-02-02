using System;
using System.Collections.Generic;

internal sealed class WheelModulePort : IModulePort, IWheelPort
{
    private WheelSimulationParamters[] wheels;
    private List<WheelSimulationParamters> wheelList = new();

    private Dictionary<Guid, int> wheelsGuidToIndex = new();
    private int nextIndex = 0;

    private readonly int wheelCount;

    public WheelModulePort(int wheelCount)
    {
        this.wheelCount = wheelCount;
    }

    #region IWheelPort
    public int RegisterWheel(Guid ID)
    {
        if (wheelsGuidToIndex.ContainsKey(ID)) throw new Exception("Duplicate wheel registration in SimulationPort");

        int index = nextIndex++;

        wheelsGuidToIndex.Add(ID, index);
        wheelList.Add(new());

        if (index == wheelCount)
        {
            wheels = wheelList.ToArray();
            wheelList.Clear();
            wheelList = null;

            wheelsGuidToIndex.Clear();
            wheelsGuidToIndex = null;
        }

        return index;
    }

    public float GetDrivetrainTorque(int ID)
    {
        return wheels[ID].drivetrainTorque;
    }

    public float GetDrivetrainRPM(int ID)
    {
        return wheels[ID].drivetrainRPM;
    }

    public float GetSuspensionNormalLoad(int ID)
    {
        return wheels[ID].suspensionNormalLoad;
    }

    public void SetFeedbackTorque(int ID, float feedbackTorque)
    {
        wheels[ID].feedbackTorque = feedbackTorque;
    }

    public void SetFeedbackRPM(int ID, float feedbackRPM)
    {
        wheels[ID].feedbackRPM = feedbackRPM;
    }
    #endregion


    #region IModulePort
    public int GetWheelCount()
    {
        return this.wheelCount;
    }

    public float GetFeedbackTorque(int ID)
    {
        return wheels[ID].feedbackTorque;
    }

    public float GetFeedbackRPM(int ID)
    {
        return wheels[ID].feedbackRPM;
    }

    public void SetDrivetrainTorque(int ID, float torque)
    {
        wheels[ID].drivetrainTorque = torque;
    }

    public void SetDrivetrainRPM(int ID, float rpm)
    {
        wheels[ID].drivetrainRPM = rpm;
    }

    public void SetSuspensionNormalLoad(int ID, float normalLoad)
    {
        wheels[ID].drivetrainRPM = normalLoad;
    }
    #endregion
}