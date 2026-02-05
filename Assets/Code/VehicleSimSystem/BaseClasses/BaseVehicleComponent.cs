internal abstract class BaseVehicleComponent<TConfig> : IVehicleComponent
where TConfig : BaseComponentConfig
{
    protected TConfig config;
    protected VehicleIOState vIOState;
    protected VehicleSimulationContext vSimCtx;

    public BaseVehicleComponent(TConfig config, VehicleIOState vIOState)
    {
        this.config = config;
        this.vIOState = vIOState;
        this.vSimCtx = vIOState.vSimCtx;
    }
}