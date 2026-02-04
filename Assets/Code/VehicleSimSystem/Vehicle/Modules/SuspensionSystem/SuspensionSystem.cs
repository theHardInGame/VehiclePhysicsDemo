using System.Threading.Tasks;
using UnityEngine;

internal sealed class SuspensionSystem : BaseVehicleModule
{
    private VehicleBody body;
    private Suspension[] suspensions;

    private VehicleBodyInput vbInput;
    private VehicleBodyOutput vbOutput;

    private readonly int suspensionCount;

    public SuspensionSystem(IModulePort modulePort, VehicleBody body, Suspension[] suspensions) : base(modulePort)
    {
        this.body = body;
        this.suspensions = suspensions;

        suspensionCount = modulePort.GetWheelCount();
        vbInput.suspensionCount = suspensionCount;
        
        for (int i = 0; i < suspensionCount; i++)
        {
            suspensions[i].ID = i;
        }
    }
    
    protected override void OnActivate()
    {
        
    }

    protected override void OnDeactivate()
    {
        
    }

    protected override void OnFixedUpdate(float fdt)
    {
        Parallel.ForEach(suspensions, suspension =>
        {
            int ID = suspension.ID;
            SuspensionInputData suspensionInput = new();
            suspensionInput.raycastLength = modulePort.GetRaycastLength(ID);
            suspensionInput.wheelRadius = modulePort.GetRadius(ID);
            suspensionInput.verticalLoad = this.vbOutput.loadPerSuspension[ID];

            SuspensionOutputData suspensionOutput = new();
            suspensionOutput = suspension.Simulate(suspensionInput, fdt);

            vbInput.isGrounded[ID] = suspensionOutput.isGrounded;
            vbInput.springRates[ID] = suspensionOutput.springRate;

            modulePort.SetSuspensionNormalLoad(ID, suspensionOutput.verticalLoad);
            modulePort.SetSuspensionForce(ID, suspensionOutput.suspensionForce);
            modulePort.SetVerticalWheelDisplacement(ID, suspensionOutput.verticalWheelDisplacement);
        });

        for (int i = 0; i < suspensionCount; i++)
        {
            vbInput.suspensionLocalPositions[i] = modulePort.GetLocalPos(i);
        }

        vbOutput = body.DistributeLoad(vbInput);
        
    }

    protected override void OnUpdate(float dt)
    {
        
    }
}