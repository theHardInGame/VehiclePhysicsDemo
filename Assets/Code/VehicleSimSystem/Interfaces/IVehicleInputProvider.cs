public interface IVehicleInputProvider
{
    float Throttle { get; }
    float Brake { get; }
    float Steer { get; }
}