using System.Collections.Generic;

internal sealed class VehicleBuilder
{
    private readonly VehicleIOState vIOState;
    private readonly SimulationPort wheelModulePort;

    internal VehicleBuilder(VehicleIOState vIOState, SimulationPort wheelModulePort)
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

        Engine engine = new Engine(config.Engine, vIOState);
        Clutch clutch = new Clutch(config.Clutch, vIOState);
        Gearbox gearbox = new Gearbox(config.Gearbox, vIOState);
        Differetial differetial = new Differetial(config.Differential, vIOState);


        List<Wheel> _wheels = new();
        List<Suspension> _suspensions = new();
        for (int i = 0; i < config.Wheels.Length; i++)
        {
            _wheels.Add(new Wheel(config.Wheels[i], vIOState, wheelModulePort));
            _suspensions.Add(new Suspension(config.Wheels[i].Suspension, vIOState));
        }

        #endregion

        #region Construct Modules
        // ===================
        // Construct Modules
        // ===================

        var _modules = new List<IVehicleModule>();

        IDrivetrainComponent[] drivetrainComponents = 
        {
            engine,
            clutch,
            gearbox
        };

        var drivetrain = new DrivetrainModule(wheelModulePort, drivetrainComponents, differetial);
        var suspension = new SuspensionSystem(wheelModulePort, _suspensions.ToArray());
        var autoShifting = new AutoShiftingModule(wheelModulePort, clutch, gearbox);


        _modules.Add(drivetrain);
        _modules.Add(suspension);
        _modules.Add(autoShifting);
        #endregion

        return new Vehicle(_wheels, _modules);
    }
}