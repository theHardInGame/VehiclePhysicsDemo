using System;
using System.Collections.Generic;

internal sealed class VehicleIOState 
{
    private Dictionary<Guid, WheelInputState> WheelInputs;
    private Dictionary<Guid, WheelOutputState> WheelOutputs;

    public VehicleCommands vehicleCommands;

    internal VehicleIOState(Dictionary<Guid, WheelInputState> WheelInputs, Dictionary<Guid, WheelOutputState> WheelOutput, VehicleCommands vehicleCommands)
    {
        this.WheelInputs = WheelInputs;
        this.WheelOutputs = WheelOutput;
        this.vehicleCommands = vehicleCommands;
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