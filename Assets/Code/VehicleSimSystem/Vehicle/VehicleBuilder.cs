using System.Collections.Generic;

internal sealed class VehicleBuilder
{
    private ISimulationContext simContext;

    internal VehicleBuilder(ISimulationContext simContext)
    {
        this.simContext = simContext;
    }

    internal Vehicle Build(VehicleConfig config)
    {
        var _components = new List<IVehicleComponent>();

        var engine = new Engine(config.Engine, simContext);
        var clutch = new Clutch(config.Clutch, simContext);

        _components.Add(engine);
        _components.Add(clutch);

        var _modules = new List<IVehicleModule>();

        var drivetrain = new DrivetrainModule(new IDrivetrainComponent[]
        {
            engine,
            clutch
        });

        _modules.Add(drivetrain);

        return new Vehicle(_components, _modules);
    }


}