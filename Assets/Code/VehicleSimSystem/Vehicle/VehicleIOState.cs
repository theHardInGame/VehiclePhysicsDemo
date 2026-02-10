using System;
using System.Collections.Generic;

internal sealed class VehicleIOState 
{
    private Dictionary<Guid, WheelInputState> WheelInputs;
    private Dictionary<Guid, WheelOutputState> WheelOutputs;

    public readonly VehicleSimulationContext vSimCtx;

    internal VehicleIOState(Dictionary<Guid, WheelInputState> WheelInputs, Dictionary<Guid, WheelOutputState> WheelOutputs, VehicleSimulationContext vSimCtx)
    {
        this.WheelInputs = WheelInputs;
        this.WheelOutputs = WheelOutputs;
        this.vSimCtx = vSimCtx;
    }

    public WheelInputState GetWheelInputState(Guid ID)
    {
        WheelInputState wheelIP;

        try
        {
            if (!WheelInputs.TryGetValue(ID, out wheelIP)) { throw new Exception($"Invalid Wheel ID \n Input-side Wheel ID: { ID }"); }
        }
        catch(Exception)
        {
            return default;
        }

        return wheelIP;
    }

    public WheelOutputState GetWheelOutputState(Guid ID)
    {
        WheelOutputState wheelOP;

        try
        {
            if (!WheelOutputs.TryGetValue(ID, out wheelOP)) { throw new Exception($"Invalid Wheel ID \n Outputs-side Wheel ID: { ID }"); }
        }
        catch(Exception)
        {
            return default;
        }

        return wheelOP;
    }
}