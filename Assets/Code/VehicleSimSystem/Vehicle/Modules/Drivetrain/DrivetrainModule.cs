internal sealed class DrivetrainModule : BaseVehicleModule
{
    private bool isActive ;
    private IDrivetrainComponent[] drivetrainComponents;
    private IJunctionComponent<DifferentialForwardState, DifferentialBackwardState> junctionComponent;

    private readonly int[] poweredWheel;
    private readonly int poweredWheelCount;
    private DifferentialBackwardState dbstate;
    private DifferentialForwardState dfstate;

    private DrivetrainBackwardState bstate;
    private DrivetrainForwardState fstate;

    public DrivetrainModule(IModulePort modulePort, IJunctionComponent<DifferentialForwardState, DifferentialBackwardState> junctionComponent, IDrivetrainComponent[] drivetrainComponents) : base(modulePort)
    {
        this.drivetrainComponents = drivetrainComponents;
        this.junctionComponent = junctionComponent;

        poweredWheel = modulePort.GetPoweredWheels();
        poweredWheelCount = poweredWheel.Length;

        dbstate = new();
        dbstate.feedbackRPMs = new float[poweredWheelCount];
        dbstate.feedbackTorques = new float[poweredWheelCount];

        dbstate = new();
        dfstate.wheelRPMs = new float[poweredWheelCount];
        dfstate.wheelTorques = new float[poweredWheelCount];
    }

    protected override void OnActivate()
    {
        isActive = true;
    }

    protected override void OnDeactivate()
    {
        isActive = false;
    }

    protected override void OnFixedUpdate(float fdt)
    {
        if (!isActive) return;

        bstate = new();

        for (int i = 0; i < poweredWheelCount; i++)
        {
            dbstate.feedbackRPMs[i] = modulePort.GetFeedbackRPM(poweredWheel[i]);
            dbstate.feedbackTorques[i] = modulePort.GetFeedbackTorque(poweredWheel[i]);
        }

        bstate = junctionComponent.Backward(dbstate, fdt).backward;

        for (int i = drivetrainComponents.Length - 1; i >= 0; i--)
        {
            bstate = drivetrainComponents[i].Backward(bstate, fdt);
        }

        fstate = new();

        for (int i = 0; i < drivetrainComponents.Length; i++)
        {
            fstate = drivetrainComponents[i].Forward(fstate, fdt);
        }

        dfstate.forward = fstate;

        dfstate = junctionComponent.Forward(dfstate, fdt);

        for (int i = 0; i < poweredWheelCount; i++)
        {
            modulePort.SetDrivetrainRPM(poweredWheel[i], dfstate.wheelRPMs[i]);
            modulePort.SetDrivetrainTorque(poweredWheel[i], dfstate.wheelTorques[i]);
        }
    }

    protected override void OnUpdate(float dt)
    {
        
    }
}