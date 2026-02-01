using UnityEngine;

public sealed class PlayerInputs : MonoBehaviour, IVehicleInputProvider
{
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnAccelerate += ReadThrottle;
        inputManager.OnBrake += ReadBrake;
        inputManager.OnSteer += ReadSteer;
    }

    private void OnDisable()
    {
        inputManager.OnAccelerate -= ReadThrottle;
        inputManager.OnBrake -= ReadBrake;
        inputManager.OnSteer -= ReadSteer;
    }

    public float Throttle { get; private set; }
    public float Brake { get; private set; }
    public float Steer { get; private set; }

    private void ReadThrottle(float t)
    {
        Throttle = t;
    }

    private void ReadBrake(float b)
    {
        Brake = b;
    }

    private void ReadSteer(float s)
    {
        Steer = s;
    }
}