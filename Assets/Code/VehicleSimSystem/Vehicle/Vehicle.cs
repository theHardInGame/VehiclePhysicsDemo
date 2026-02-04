using System.Collections.Generic;
using System.Threading.Tasks;

internal sealed class Vehicle
{
    private readonly IEnumerable<IVehicleComponent> components;
    private readonly IEnumerable<IVehicleModule> modules;

    internal Vehicle(IEnumerable<IVehicleComponent> components, IEnumerable<IVehicleModule> modules)
    {
        this.components = components;
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
        Parallel.ForEach(modules, module =>
        {
            module.FixedUpdate(fdt);
        });
    }

    internal void Update(float dt)
    {
        foreach (var module in modules)
        {
            module.Update(dt);
        }
    }
}