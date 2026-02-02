using System.Collections.Generic;

internal sealed class VehicleBuilder
{
    private VehicleIOState vIOState;
    private WheelModulePort wheelModulePort;

    internal VehicleBuilder(VehicleIOState vIOState, WheelModulePort wheelModulePort)
    {
        this.vIOState = vIOState;
        this.wheelModulePort = wheelModulePort;
    }

    internal Vehicle Build(VehicleConfig config)
    {
        #region Construct Components
        // ======================
        // Construct Components
        // ======================

        var _components = new List<IVehicleComponent>();

        Engine engine = new Engine(config.Engine, vIOState);
        Clutch clutch = new Clutch(config.Clutch, vIOState);
        Gearbox gearbox = new Gearbox(config.Gearbox, vIOState);
        Differetial differetial = new Differetial(config.Differential, vIOState);

        _components.Add(engine);
        _components.Add(clutch);
        _components.Add(gearbox);
        _components.Add(differetial);

        for (int i = 0; i < config.Wheels.Length; i++)
        {
            _components.Add(new Wheel(config.Wheels[i], vIOState, wheelModulePort));
        }

        #endregion

        #region Construct Modules
        // ===================
        // Construct Modules
        // ===================

        var _modules = new List<IVehicleModule>();

        var drivetrain = new DrivetrainModule(wheelModulePort, new IDrivetrainComponent[]
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