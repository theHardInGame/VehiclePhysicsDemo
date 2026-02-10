internal sealed class SuspensionSystem : BaseVehicleModule
{
    private Suspension[] suspensions;
    private readonly int suspensionCount;

    public SuspensionSystem(IModuleSimulationPort modulePort, Suspension[] suspensions) : base(modulePort)
    {
        this.suspensions = suspensions;
        suspensionCount = modulePort.GetWheelCount();
    }
    
    protected override void OnActivate() {  }
    protected override void OnDeactivate() { }

    protected override void OnFixedUpdate(float fdt)
    {
        if (!isActive) return;

        for (int i = 0; i < suspensionCount; i++)
        {
            float currentLength = moduleSimPort.GetSuspensionLength(i);
            float springRelativeVelocity = moduleSimPort.GetSpringRelativeVelocity(i);
            float suspensionForce = suspensions[i].Simulate(currentLength, springRelativeVelocity, fdt);
            moduleSimPort.SetSuspensionForce(i, suspensionForce);
        }
    }

    protected override void OnUpdate(float dt) { }
}