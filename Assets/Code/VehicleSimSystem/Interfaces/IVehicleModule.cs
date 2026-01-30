internal interface IVehicleModule
{
    void Update(float deltaTime);
    void FixedUpdate(float fixedDeltaTime);
    void Activate();
    void Deactivate();
}