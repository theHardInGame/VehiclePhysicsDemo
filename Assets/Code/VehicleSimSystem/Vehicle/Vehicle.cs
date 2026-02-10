using System.Collections.Generic;
using System.Threading.Tasks;

internal sealed class Vehicle
{
    private readonly IEnumerable<Wheel> wheels;
    private readonly IEnumerable<IVehicleModule> modules;

    internal Vehicle(IEnumerable<Wheel> wheels, IEnumerable<IVehicleModule> modules)
    {
        this.wheels = wheels;
        this.modules = modules;
    }

    internal void Activate()
    {
        foreach (var module in modules)
        {
            module.Activate();
        }
    }

    internal void Deactivate()
    {
        foreach (var module in modules)
        {
            module.Deactivate();
        }
    }

    internal void FixedUpdate(float fdt)
    {     
        foreach(Wheel wheel in wheels)
        {
            wheel.Setup();
        }

        Parallel.ForEach(modules, module =>
        {
            module.FixedUpdate(fdt);
        });
        
        foreach(Wheel wheel in wheels)
        {
            wheel.Simulate(fdt);
        }
    }

    internal void Update(float dt)
    {
        foreach (var module in modules)
        {
            module.Update(dt);
        }
    }
}