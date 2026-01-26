using UnityEngine;
using com.thehardingame.bootstrap;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour, IInitializable
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private InputActionAsset inputActionAsset;

    public async Task InitializeAsync()
    {
        await Awaitable.MainThreadAsync();

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        if (inputActionAsset == null)
        {
            Debug.LogError("InputActionAsset is null. Check SerializedField in inspector");
        }

        SetUpControls();

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void OnEnable()
    {
        EnableVehicleActions();
    }

    private void OnDisable()
    {
        DisableVehicleActions();
    }



    [SerializeField] private string mapName = "Vehicle";
    [SerializeField] private string accelerateActionName = "Accelerate";
    [SerializeField] private string brakeActionName = "Brake";
    [SerializeField] private string steerActionName = "Steer";

    private InputActionMap actionMap;

    private InputAction Accelerate;
    private InputAction Brake;
    private InputAction Steer;

    public event Action<float> OnAccelerate;
    public event Action<float> OnBrake;
    public event Action<float> OnSteer;

    private void SetUpControls()
    {
        actionMap = FindInputActionMap(mapName);

        Accelerate = FindInputAction(accelerateActionName, actionMap);
        Brake = FindInputAction(brakeActionName, actionMap);
        Steer = FindInputAction(steerActionName, actionMap);

        RegisterInputAction(Accelerate, f => OnAccelerate?.Invoke(f), 0.0f);
        RegisterInputAction(Brake, f => OnBrake?.Invoke(f), 0.0f);
        RegisterInputAction(Steer, f => OnSteer?.Invoke(f), 0.0f);

        EnableVehicleActions();
    }

    public void EnableVehicleActions()
    {
        Accelerate?.Enable();
        Brake?.Enable();
        Steer?.Enable();
    }

    public void DisableVehicleActions()
    {
        Accelerate?.Disable();
        Brake?.Disable();
        Steer?.Disable();
    }






    /// <summary>
    /// Finds and returns InputActionMap based on string passed
    /// </summary>
    /// <param name="mapName">Name of InputActionMap. Must not be null or empty</param>
    /// <returns>InputActionMap</returns>
    private InputActionMap FindInputActionMap(string mapName)
    {
        if(!string.IsNullOrEmpty(mapName))
        {
            InputActionMap map = inputActionAsset.FindActionMap(mapName);

            if(map != null)
            {
                return map;
            }
            else
            {
                Debug.LogError($"Could not find InputActionMap: {mapName}");
                return null;
            }
        }

        Debug.LogError($"ActionMapName is null/empty! + \n + {Environment.StackTrace}");
        return null;
    }

    /// <summary>
    /// Finds and Input Action based on string and map passed
    /// </summary>
    /// <param name="actionName">Name of InputAction. Must not be null or empty</param>
    /// <param name="map">InputActionMap to find InputAction in. Must not be null</param>
    /// <returns>InputAction</returns>
    private InputAction FindInputAction(string actionName, InputActionMap map)
    {
        if (!string.IsNullOrEmpty(actionName))
        {
            if (map == null)
            {
                Debug.LogError($"Trying to find Action '{actionName}' in a null InputActionMap '{map}'");
                return null;
            }

            InputAction action = map.FindAction(actionName);

            if (action != null)
            {
                return action;
            }
            else
            {
                Debug.LogError($"Could not find InputAction: {actionName} in InputActionMap {map.name}");
                return null;
            }
        }

        Debug.LogError($"Action name is null/empty! + \n + {Environment.StackTrace}");
        return null;
    }

    /// <summary>
    /// Registers an InputAction to a public reference using a setter delegate. <br/>
    /// <para>
    /// Example use: <br/>
    /// RegisterInputAction(Jump, f => JumpEvent?.Invoke(f), 0.0f) <br/>
    /// Where:
    /// <list type="bullet">
    ///     <item> Jump: InputAction </item>
    ///     <item> JumpEvent: public event </item>
    /// </list>
    /// </para>
    /// </summary>
    /// <typeparam name="T">Type of input. Must be a value type (struct).</typeparam>
    /// <param name="inputAction">The InputAction to register. Must not be null.</param>
    /// <param name="callback">Delegate to assign the value to your field.</param>
    /// <param name="defaultValue">Value assigned when the action is canceled.</param>
    private void RegisterInputAction<T>(InputAction inputAction, Action<T> callback, T defaultValue) where T : struct
    {
        if (inputAction == null)
        {
            Debug.LogError($"Attempted to register null Action + \n + {Environment.StackTrace}");
            return;
        }

        inputAction.performed += context => callback(context.ReadValue<T>());
        inputAction.canceled += context => callback(defaultValue);
    }
}
