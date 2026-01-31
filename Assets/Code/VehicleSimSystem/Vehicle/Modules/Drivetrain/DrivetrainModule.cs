internal sealed class DrivetrainModule : BaseVehicleModule
{
    private bool isActive ;
    private IDrivetrainComponent[] _drivetrainComponents;
    internal DrivetrainModule(IDrivetrainComponent[] drivetrainComponents)
    {
        _drivetrainComponents = drivetrainComponents;
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
        
        ForwardState fstate = new();

        for (int i = 0; i < _drivetrainComponents.Length; i++)
        {
            fstate = _drivetrainComponents[i].Forward(fstate, fdt);
        }

        BackwardState bstate = new();

        for (int i = _drivetrainComponents.Length - 1; i >= 0; i--)
        {
            bstate = _drivetrainComponents[i].Backward(bstate, fdt);
        }
    }

    protected override void OnUpdate(float dt)
    {
        
    }
}