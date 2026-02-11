# TODO

> ## 30/01/2026
1. [x] `31/01/2026` Create System and Module architecture.
2. [x] `31/01/2026` Remove Update-FixedUpdate (And other Monobehaviour entries) from BaseModule, and move them to Systems.
3. [x] `31/01/2026` ModuleHandler -> SystemHandler. Manage various System's runtime.
4. [x] `31/01/2026` Restructure folders for new System-Module architecture.

> ## 31/01/2026
1. [x] `01/02/2026` Implement drivetrain module.
2. [x] `01/02/2026` Figure out Monobehaviour linking of module runtimes.
3. [ ] `ABORTED` List out required remaining modules and figure out implementation design.

Ignore GameObject interaction for now. Deal with internal simulation first.

> ## 01/02/2026
1. [ ] `ABORTED` Design Wheel component (for drivetrain).
2. [x] `02/02/2026` Design Simulation-Gameobject data exchanging.

> ## 03/02/2026
1. [x] `04/02/2026` Design Suspension around new WheelModulePort design.
2. [x] `03/02/2026` Complete Drivetrain modules.
3. [x] `04/02/2026` Complete Suspension module.
4. [x] `11/02/2026` Complete Wheel.

> ## 04/02/2026
1. [x] `11/02/2026` Implement Unity side implementation for testing.
2. [ ] Convert helpers to static methods.
3. [x] `11/02/2026` Implement Steering.
4. [x] `11/02/2026` Decide where Brakes live and then implement.
5. [x] `11/02/2026` Implement smooth input handling in `VehicleController`.

## 11/02/2026
1. [ ] Fix sliding/jitter on low/near zero speed.
2. [ ] Fix infinite acceleration.
3. [ ] Fix steer grip.
4. [ ] Fix acceleration on steering.
5. [ ] Implement auto shifting.
6. [ ] Design ackerman-esque generalized steering.
7. [ ] Clean up messy Unity-Simulation data exchange side implementations.
8. [ ] Clean up `WheelMB` with streamlined methods.
9. [ ] Merge `WheelMB`'s `wheelCollider` and `wheelDisplace`.

