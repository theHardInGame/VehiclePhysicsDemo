internal abstract class BaseVehicleComponent<TConfig> : IVehicleComponent
where TConfig : BaseComponentConfig
{
    protected TConfig config;
    protected VehicleSimulationContext vSimCtx;

    public BaseVehicleComponent(TConfig config, VehicleSimulationContext vSimCtx)
    {
        this.config = config;
        this.vSimCtx = vSimCtx;
    }
}