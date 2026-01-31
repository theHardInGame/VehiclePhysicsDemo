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
        #region Construct Components
        // ======================
        // Construct Components
        // ======================

        var _components = new List<IVehicleComponent>();

        Engine engine = new Engine(config.Engine, simContext);
        Clutch clutch = new Clutch(config.Clutch, simContext);
        Gearbox gearbox = new Gearbox(config.Gearbox, simContext);
        Differetial differetial = new Differetial(config.Differential, simContext);

        _components.Add(engine);
        _components.Add(clutch);
        _components.Add(gearbox);
        _components.Add(differetial);
        #endregion

        #region Construct Modules
        // ===================
        // Construct Modules
        // ===================

        var _modules = new List<IVehicleModule>();

        var drivetrain = new DrivetrainModule(new IDrivetrainComponent[]
        {
            engine,
            clutch,
            gearbox,
            differetial
        });

        _modules.Add(drivetrain);
        #endregion

        return new Vehicle(_components, _modules);
    }
}