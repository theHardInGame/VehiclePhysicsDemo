using System;
using System.Collections.Generic;
using System.Numerics;

internal sealed class WheelModulePort : IModulePort, IWheelPort
{
    private WheelSimulationParamters[] wheels;
    private List<WheelSimulationParamters> wheelList = new();

    private Dictionary<Guid, int> wheelsGuidToIndex = new();
    private int nextIndex = 0;

    private readonly int wheelCount;
    private readonly List<int> poweredWheels;
    private readonly List<int> steeredWheels;

    public WheelModulePort(int wheelCount)
    {
        this.wheelCount = wheelCount;
    }

    #region IWheelPort
    public int RegisterWheel(Guid ID, bool isPowered, bool isSteered, float radius)
    {
        if (wheelsGuidToIndex.ContainsKey(ID)) throw new Exception("Duplicate wheel registration in SimulationPort");

        int index = nextIndex++;

        wheelsGuidToIndex.Add(ID, index);
        wheelList.Add(new(isPowered, isSteered, radius));

        if (isPowered) { poweredWheels.Add(index); }
        if (isSteered) { steeredWheels.Add(index); }

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

    public void SetLocalPos(int ID, Vector3 localPos)
    {
        wheels[ID].localPos = localPos;
    }

    public void SetGroundHeight(int ID, float height)
    {
        wheels[ID].raycastLength = height;
    }
    #endregion


    #region IModulePort
    public int GetWheelCount()
    {
        return wheelCount;
    }

    public int[] GetPoweredWheels()
    {
        return poweredWheels.ToArray();
    }

    public int[] GetSteeredWheels()
    {
        return steeredWheels.ToArray();
    }

    public bool IsSteeredWheel(int ID)
    {
        return wheels[ID].isSteered;
    }

    public float GetFeedbackTorque(int ID)
    {
        return wheels[ID].feedbackTorque;
    }

    public float GetFeedbackRPM(int ID)
    {
        return wheels[ID].feedbackRPM;
    }

    public Vector3 GetLocalPos(int ID)
    {
        return wheels[ID].localPos;
    }

    public float GetRadius(int ID)
    {
        return wheels[ID].radius;
    }

    public float GetRaycastLength(int ID)
    {
        return wheels[ID].raycastLength;
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
        wheels[ID].suspensionNormalLoad = normalLoad;
    }

    public void SetSuspensionForce(int ID, float force)
    {
        wheels[ID].suspensionForce = force;
    }

    public void SetVerticalWheelDisplacement(int ID, float displacement)
    {
        wheels[ID].verticalWheelDisplacement = displacement;
    }
    #endregion
}