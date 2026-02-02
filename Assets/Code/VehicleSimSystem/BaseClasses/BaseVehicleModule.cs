internal abstract class BaseVehicleModule : IVehicleModule
{
    IModulePort modulePort;

    public BaseVehicleModule(IModulePort modulePort)
    {
        this.modulePort = modulePort;
    }

    public void Activate()
    {
        OnActivate();
    }

    public void Deactivate()
    {
        OnDeactivate();
    }

    public void FixedUpdate(float fdt)
    {
        OnFixedUpdate(fdt);
    }

    public void Update(float dt)
    {
        OnUpdate(dt);
    }

    protected abstract void OnActivate();
    protected abstract void OnDeactivate();
    protected abstract void OnFixedUpdate(float fdt);
    protected abstract void OnUpdate(float dt);
}