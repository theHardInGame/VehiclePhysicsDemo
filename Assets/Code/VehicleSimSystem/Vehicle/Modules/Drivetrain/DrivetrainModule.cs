internal sealed class DrivetrainModule : BaseVehicleModule
{
    private IDrivetrainComponent[] _drivetrainComponents;
    internal DrivetrainModule(IDrivetrainComponent[] drivetrainComponents)
    {
        _drivetrainComponents = drivetrainComponents;
    }

    protected override void OnActivate()
    {
        
    }

    protected override void OnDeactivate()
    {
        
    }

    protected override void OnFixedUpdate(float fixedDeltaTime)
    {
        
    }

    protected override void OnUpdate(float deltaTime)
    {
        
    }
}