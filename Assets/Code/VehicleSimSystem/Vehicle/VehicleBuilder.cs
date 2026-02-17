using System.Collections.Generic;

internal sealed class VehicleBuilder
{
    private readonly VehicleSimulationContext vSimCtx;
    private readonly SimulationPort wheelModulePort;

    internal VehicleBuilder(VehicleSimulationContext vSimCtx, SimulationPort wheelModulePort)
    {
        this.vSimCtx = vSimCtx;
        this.wheelModulePort = wheelModulePort;
    }

    internal Vehicle Build(VehicleConfig config)
    {
        #region Construct Components
        // ======================
        // Construct Components
        // ======================

        Engine engine = new(config.Engine, vSimCtx);
        Clutch clutch = new(config.Clutch, vSimCtx);
        Gearbox gearbox = new(config.Gearbox, vSimCtx);
        Differetial differetial = new(config.Differential, vSimCtx);


        List<Wheel> _wheels = new();
        List<Suspension> _suspensions = new();
        for (int i = 0; i < config.Wheels.Length; i++)
        {
            _wheels.Add(new Wheel(config.Wheels[i], vSimCtx, wheelModulePort));
            _suspensions.Add(new Suspension(config.Wheels[i].Suspension, vSimCtx));
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
        //var autoShifting = new AutoShiftingModule(wheelModulePort, clutch, gearbox);


        _modules.Add(drivetrain);
        _modules.Add(suspension);
        //_modules.Add(autoShifting);
        #endregion

        return new Vehicle(_wheels, _modules);
    }
}