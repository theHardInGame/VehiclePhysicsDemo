internal abstract class BaseVehicleModule : IVehicleModule
{
    protected IModulePort moduleSimPort;
    protected bool isActive = false;
    public BaseVehicleModule(IModulePort modulePort)
    {
        this.moduleSimPort = modulePort;
    }

    public void Activate()
    {
        isActive = true;
        OnActivate();
    }

    public void Deactivate()
    {
        isActive = false;
        OnDeactivate();
    }

    public void FixedUpdate(float fdt)
    {
        if (!isActive) return;

        OnFixedUpdate(fdt);
    }

    public void Update(float dt)
    {
        if (!isActive) return;
        OnUpdate(dt);
    }

    protected abstract void OnActivate();
    protected abstract void OnDeactivate();
    protected abstract void OnFixedUpdate(float fdt);
    protected abstract void OnUpdate(float dt);
}