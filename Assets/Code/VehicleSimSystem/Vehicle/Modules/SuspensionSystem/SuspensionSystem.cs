using System.Threading.Tasks;

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
        body.suspensionCount = suspensionCount;

        for (int i = 0; i < suspensionCount; i++)
        {
            suspensions[i].ID = i;
            suspensions[i].vectorFromCG = body.vectorsToSuspension[i] = modulePort.GetLocalPos(i) - body.localCG;
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
        vbOutput = body.DistributeLoad(vbInput);

        Parallel.ForEach(suspensions, suspension =>
        {
            int ID = suspension.ID;
            SuspensionInputData suspensionInput = new()
            {
                raycastLength = modulePort.GetRaycastLength(ID),
                raycastNormal = modulePort.GetRaycastNormal(ID),
                wheelRadius = modulePort.GetRadius(ID),
                staticNormalForce = vbOutput.staticLoadPerSuspension[ID]
            };

            SuspensionOutputData suspensionOutput = new();
            suspensionOutput = suspension.Simulate(suspensionInput, fdt);

            vbInput.isGrounded[ID] = suspensionOutput.isGrounded;
            vbInput.springRates[ID] = suspensionOutput.springRate;
            vbInput.maxCompressed[ID] = suspensionOutput.maxCompressed;
            vbInput.suspensionNormals[ID] = modulePort.GetRaycastNormal(ID);

            modulePort.SetSuspensionNormalForce(ID, suspensionOutput.normalForce);
            modulePort.SetSuspensionForce(ID, suspensionOutput.suspensionForce);
            modulePort.SetVerticalWheelDisplacement(ID, suspensionOutput.verticalWheelDisplacement);
        });
    }

    protected override void OnUpdate(float dt)
    {
        
    }
}