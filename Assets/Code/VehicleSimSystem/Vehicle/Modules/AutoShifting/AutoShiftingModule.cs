internal sealed class AutoShiftingModule : BaseVehicleModule
{
    private readonly IASClutch clutch;
    private readonly IASGearbox gearbox;

    public AutoShiftingModule(IModuleSimulationPort modulePort, IASClutch clutch, IASGearbox gearbox) : base(modulePort)
    {
        this.clutch = clutch;
        this.gearbox = gearbox;
    }

    protected override void OnActivate()
    {
        gearbox.RequestShift += RegisterShiftReqeust;
    }

    protected override void OnDeactivate()
    {
        gearbox.RequestShift -= RegisterShiftReqeust;
    }

    protected override void OnFixedUpdate(float fdt)
    {
        clutch.Tick(fdt);

        /*switch (shiftRequest)
        {
            case ShiftRequest.Upshift:
                clutch.StartAutoShift();
                gearbox.Upshift();
                break;

            case ShiftRequest.Downshift:
                clutch.StartAutoShift();
                gearbox.Downshift();
                break;
        }*/
    }

    protected override void OnUpdate(float dt)
    {
        
    }


    private ShiftRequest shiftRequest;
    private void RegisterShiftReqeust(ShiftRequest request)
    {
        shiftRequest = request;
    }
}