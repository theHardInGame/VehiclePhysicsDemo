internal abstract class BaseVehicleModule : IVehicleModule
{
    public void Activate()
    {
        OnActivate();
    }

    public void Deactivate()
    {
        OnDeactivate();
    }

    public void FixedUpdate(float fixedDeltaTime)
    {
        OnFixedUpdate(fixedDeltaTime);
    }

    public void Update(float deltaTime)
    {
        OnUpdate(deltaTime);
    }

    protected abstract void OnActivate();
    protected abstract void OnDeactivate();
    protected abstract void OnFixedUpdate(float fixedDeltaTime);
    protected abstract void OnUpdate(float deltaTime);
}