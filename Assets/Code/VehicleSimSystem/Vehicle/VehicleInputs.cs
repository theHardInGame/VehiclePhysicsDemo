internal sealed class VehicleInputs
{
    private float throttle;
    private float brake;
    private float steering;

    internal float GetThrottle() => throttle;
    internal float GetBrake() => brake;
    internal float GetSteering() => steering;

    internal void SetThrottle(float throttle) { this.throttle = throttle; }
    internal void SetBrake(float brake) { this.brake = brake; }
    internal void SetSteering(float steering) { this.steering = steering; }
}