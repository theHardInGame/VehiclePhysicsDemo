public abstract class BaseVehicleModule<TConfig> : IVehicleModule
where TConfig : BaseModuleConfig
{
    protected TConfig config;

    public BaseVehicleModule(TConfig config)
    {
        this.config = config;
    }

    public virtual void OnActivate() {}

    public virtual void OnDeactivate() {}

    public virtual void OnFixedUpdate(float fixedDeltaTime) { }

    public virtual void OnUpdate(float deltaTime) {}
}