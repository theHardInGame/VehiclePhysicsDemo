using System.Numerics;

internal sealed class VehicleSimulationContext
{
    #region Input States
    // ==============
    // Input States
    // ==============
    
    private float throttle;
    private float brake;
    private float steering;

    internal float GetThrottle() => throttle;
    internal float GetBrake() => brake;
    internal float GetSteering() => steering;

    internal void SetThrottle(float throttle) { this.throttle = throttle; }
    internal void SetBrake(float brake) { this.brake = brake; }
    internal void SetSteering(float steering) { this.steering = steering; }
    #endregion


    #region Static States
    // ===============
    // Static States
    // ===============

    private float mass;
    internal float GetMass() => mass;
    internal void SetMass(float mass) { this.mass = mass; }
    #endregion


    #region Dynamic States
    // ================
    // Dynamic States
    // ================

    private Vector3 cgLocal;
    private Vector3 velocity;

    internal Vector3 GetCGLocal() => cgLocal;
    internal Vector3 GetVelocity() => velocity;

    internal void SetCGLocal(Vector3 cgLocal) { this.cgLocal = cgLocal; }
    internal void SetVelocity(Vector3 velocity) { this.velocity = velocity; }
    #endregion
}