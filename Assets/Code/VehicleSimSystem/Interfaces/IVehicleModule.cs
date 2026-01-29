public interface IVehicleModule
{
    void OnUpdate(float deltaTime);
    void OnFixedUpdate(float fixedDeltaTime);
    void OnActivate();
    void OnDeactivate();
}