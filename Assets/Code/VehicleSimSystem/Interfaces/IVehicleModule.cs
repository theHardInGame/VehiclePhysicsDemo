
public interface IVehicleModule
{
    void OnStart();
    void OnUpdate(float deltaTime);
    void OnFixedUpdate(float fixedDeltaTime);
    void OnActivate();
    void OnDeactivate();
}