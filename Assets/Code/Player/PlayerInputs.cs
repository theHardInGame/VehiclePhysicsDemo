using UnityEngine;

public sealed class PlayerInputs : MonoBehaviour, IVehicleInputProvider
{
    private InputManager inputManager;

    [SerializeField]
    private bool useManualControl;
    private bool isUsingManualControl;

    [SerializeField, Range(0, 1)]
    private float throttle;
    
    [SerializeField, Range(0, 1)]
    private float brake;
    
    [SerializeField, Range(-1, 1)]
    private float steer;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        EnableInputManager();
    }

    private void OnDisable()
    {
        DisableInputManager();
    }

    private void Update()
    {
        if (useManualControl && !isUsingManualControl)
        {
            DisableInputManager();
            isUsingManualControl = useManualControl;
        }

        if (!useManualControl && isUsingManualControl)
        {
            EnableInputManager();
            isUsingManualControl = useManualControl;
        }

        ManualControl();
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

    private void DisableInputManager()
    {
        inputManager.OnAccelerate -= ReadThrottle;
        inputManager.OnBrake -= ReadBrake;
        inputManager.OnSteer -= ReadSteer;
    }

    private void EnableInputManager()
    {
        inputManager.OnAccelerate += ReadThrottle;
        inputManager.OnBrake += ReadBrake;
        inputManager.OnSteer += ReadSteer;
    }

    private void ManualControl()
    {
        if (!isUsingManualControl) return;

        Throttle = throttle;
        Brake = brake;
        Steer = steer;
    }
}