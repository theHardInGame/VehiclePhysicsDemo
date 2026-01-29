using UnityEngine;

public class VehicleController : MonoBehaviour, IVehicleInput
{
    private InputManager inputManager;

    [SerializeField] private VehicleModuleHandler vehicleModuleHandler;

    [Header("Vehicle Module Configs")]
    [SerializeField] private EngineConfig engineConfig;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnAccelerate += OnThrottleInput;
        inputManager.OnBrake += OnBrakeInput;
        inputManager.OnSteer += OnSteerInput;
    }

    public float Throttle { get; private set; }
    public float Brake { get; private set; }
    public float Steer { get; private set; }

    private void OnThrottleInput(float throttle)
    {
        Throttle = throttle;
    }

    private void OnSteerInput(float steer)
    {
        Brake = steer;
    }

    private void OnBrakeInput(float brake)
    {
        Steer = brake;
    }
}