internal interface IVehicleModule
{
    void Update(float dt);
    void FixedUpdate(float fdt);
    void Activate();
    void Deactivate();
}