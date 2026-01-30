internal abstract class BaseVehicleComponent<TConfig> : IVehicleComponent
where TConfig : BaseComponentConfig
{
    protected TConfig config;
    protected ISimulationContext simContext;

    public BaseVehicleComponent(TConfig config, ISimulationContext simContext)
    {
        this.config = config;
        this.simContext = simContext;
    }
}