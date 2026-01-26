using UnityEngine;

public class VehicleModuleHandler : MonoBehaviour
{
    [SerializeField] private VehicleController vehicleController;

    private IVehicleModule[] vehicleModules;

    private void Start()
    {
        for (int i = 0; i < vehicleModules.Length; i++)
        {
            vehicleModules[i].OnStart();
        }
    }

    private void Update()
    {
        for (int i = 0; i < vehicleModules.Length; i++)
        {
            vehicleModules[i].OnUpdate(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < vehicleModules.Length; i++)
        {
            vehicleModules[i].OnFixedUpdate(Time.fixedDeltaTime);
        }
    }
}