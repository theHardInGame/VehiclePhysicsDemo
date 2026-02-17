internal sealed class DrivetrainModule : BaseVehicleModule
{
    private readonly int[] poweredWheels;
    private readonly int poweredWheelCount;

    private readonly Differetial differetial;

    private readonly IDrivetrainComponent[] drivetrainComponent;

    public DrivetrainModule(IModulePort modulePort, IDrivetrainComponent[] drivetrainComponent, Differetial differetial) : base(modulePort)
    {
        this.drivetrainComponent = drivetrainComponent;
        this.differetial = differetial;

        poweredWheels = modulePort.GetPoweredWheels();
        poweredWheelCount = poweredWheels.Length;
    }

    protected override void OnActivate() { }

    protected override void OnDeactivate() { }

    protected override void OnFixedUpdate(float fdt)
    {       
        float[] wheelTorques = new float[poweredWheelCount];
        for (int i = 0; i < poweredWheelCount; i++)
        {
            wheelTorques[i] = moduleSimPort.GetWheelTorque(poweredWheels[i]);
        }

        float backwardInput = differetial.CombineTorque(wheelTorques);
        for (int i = drivetrainComponent.Length - 1; i >= 0; i--)
        {
            backwardInput = drivetrainComponent[i].SimulateBackwardTorque(backwardInput, fdt);
        }

        float forwardInput = 0;
        for (int i = 0; i < drivetrainComponent.Length; i++)
        {
            forwardInput = drivetrainComponent[i].SimulateForwardTorque(forwardInput, fdt);
        }

        float[] wheelRPMs = new float[poweredWheelCount];
        for (int i = 0; i < poweredWheelCount; i++)
        {
            wheelRPMs[i] = moduleSimPort.GetWheelRPM(poweredWheels[i]);
        }

        float[] Tws = differetial.SplitTorque(forwardInput, wheelRPMs);

        for (int i = 0; i < poweredWheelCount; i++)
        {
            moduleSimPort.SetDrivetrainTorque(poweredWheels[i], Tws[i]);
        }
    }

    protected override void OnUpdate(float dt) { }
}