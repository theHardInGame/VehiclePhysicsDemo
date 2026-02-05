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

    private Vector3 gravity;
    private float mass;
    internal float GetMass() => mass;
    internal Vector3 GetGravity() => gravity;
    internal void SetMass(float mass) { this.mass = mass; }
    internal void SetGravity(Vector3 gravity) { this.gravity = gravity; }
    #endregion


    #region Dynamic States
    // ================
    // Dynamic States
    // ================

    private Vector3 cgLocal;
    private Vector3 rbdLinearAcceleration;
    private Vector3 rbdAngularVelocity;
    private Vector3 rbdAngularAcceleration;

    internal Vector3 GetCGLocal() => cgLocal;
    internal Vector3 GetLinearAcceleartion() => rbdLinearAcceleration;
    internal Vector3 GetAngularVelocity() => rbdAngularVelocity;
    internal Vector3 GetAngularAcceleration() => rbdAngularAcceleration;

    internal void SetCGLocal(Vector3 cgLocal) { this.cgLocal = cgLocal; }
    internal void SetLinearAcceleration(Vector3 rbdLinearAcceleration) { this.rbdLinearAcceleration = rbdLinearAcceleration; }
    internal void SetAngularVelocity(Vector3 rbdAngularVelocity) { this.rbdAngularVelocity = rbdAngularVelocity; }
    internal void SetAngularAcceleration(Vector3 rbdAngularAcceleration) { this.rbdAngularAcceleration = rbdAngularAcceleration; }
    #endregion
}