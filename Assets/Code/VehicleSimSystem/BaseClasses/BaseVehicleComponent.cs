internal abstract class BaseVehicleComponent<TConfig> : IVehicleComponent
where TConfig : BaseComponentConfig
{
    protected TConfig config;
    protected VehicleIOState vIOState;

    public BaseVehicleComponent(TConfig config, VehicleIOState vIOState)
    {
        this.config = config;
        this.vIOState = vIOState;
    }
}