# Vehicle Physics Simulation System

A comprehensive vehicle physics simulation system built for Unity, focusing on realistic drivetrain, suspension, and wheel dynamics.

## Overview

This project implements a modular vehicle simulation system with separate concerns for drivetrain mechanics, suspension dynamics, and wheel physics. The architecture uses a component-based design with clear separation between simulation logic and Unity integration.

![Demo](demo.gif)
## Core Systems

### Drivetrain Module
Simulates the complete powertrain from engine to wheels:
- **Engine** - Torque generation based on RPM and throttle input
- **Clutch** - Engagement/disengagement with auto-shift support (WIP)
- **Gearbox** - Multi-gear transmission with automatic upshift/downshift
- **Differential** - Torque distribution to powered wheels with slip coupling

**Key Files:**
- [DrivetrainModule.cs](Assets/Code/VehicleSimSystem/Vehicle/Modules/Drivetrain/DrivetrainModule.cs)
- [Engine.cs](Assets/Code/VehicleSimSystem/Vehicle/Components/Engine/Engine.cs)
- [Gearbox.cs](Assets/Code/VehicleSimSystem/Vehicle/Components/Gearbox/Gearbox.cs)
- [Clutch.cs](Assets/Code/VehicleSimSystem/Vehicle/Components/Clutch/Clutch.cs)
- [Differential.cs](Assets/Code/VehicleSimSystem/Vehicle/Components/Differential/Differential.cs)

### Suspension System
Calculates suspension forces based on compression and velocity:
- Spring and damper force calculations
- Suspension length clamping
- Per-wheel suspension components

**Key Files:**
- [SuspensionSystem.cs](Assets/Code/VehicleSimSystem/Vehicle/Modules/SuspensionSystem/SuspensionSystem.cs)
- [Suspension.cs](Assets/Code/VehicleSimSystem/Vehicle/Components/Suspension/Suspension.cs)

### Wheel Physics
Simulates individual wheel behavior:
- Longitudinal and lateral slip calculations
- Tire force generation based on slip curves
- Wheel inertia and angular velocity
- Rolling resistance and brake torque

**Key Files:**
- [Wheel.cs](Assets/Code/VehicleSimSystem/Vehicle/Wheel/Wheel.cs)

## Input & Control

Input handling is separated from simulation logic through the `IVehicleInputProvider` interface:

- **Throttle** - Engine power delivery (0-1)
- **Brake** - Braking force application (0-1)
- **Steer** - Steering angle control (-1 to 1)

Default implementation: [PlayerInputs.cs](Assets/Code/Player/PlayerInputs.cs) (uses Unity Input System via `Assets/Code/Managers/InputManager.cs`)

## Architecture

### Module-Component Pattern

```
Vehicle
├── Modules (runtime simulation)
│   ├── DrivetrainModule
│   ├── SuspensionSystem
│   └── AutoShiftingModule
└── Components (isolated logic)
    ├── Engine
    ├── Clutch
    ├── Gearbox
    ├── Differential
    ├── Suspension (per-wheel)
    └── Wheel (per-wheel)
```

### Data Flow

1. **Input Phase** - `PlayerInputs` reads player input
2. **Update Phase** - `VehicleController` updates simulation context
3. **Fixed Simulation** - `Vehicle.FixedUpdate()` runs:
   - Wheel setup (read input states)
   - Module updates (parallel)
   - Wheel simulation (compute forces)
4. **Wheel Application** - `WheelMB` applies forces to physics bodies

### Port Pattern

Communication between simulation and wheels uses port interfaces:
- `IModuleSimulationPort` - Module → Wheel data
- `IWheelSimulationPort` - Wheel → Module data

## Configuration

Vehicle behavior is defined through ScriptableObject configs:

- `VehicleConfig` - Master vehicle settings
- `EngineConfig` - Engine torque curves
- `GearboxConfig` - Gear ratios and shift points
- `WheelConfig` - Tire slip curves and suspension
- `SuspensionConfig` - Spring/damper rates

## Key Classes

| Class                         | Purpose                           |
|-------------------------------|-----------------------------------|
| `VehicleController`           | Main MonoBehaviour coordinator    |
| `Vehicle`                     | Simulation orchestrator           | 
| `VehicleSimulationContext`    | Shared state container            |
| `VehicleIOState`              | Wheel I/O state management        |
| `SimulationPort`              | Port implementation               |
| `WheelOuputState`             | Wheel's output state for runtime  |
| `WheemInputState`             | Wheel's input state from runtime  |
## Usage

1. Create a `VehicleConfig` ScriptableObject with engine, drivetrain, and wheel configs
2. Assign config to `VehicleController`
3. Set up wheel GameObjects with `WheelMB` components
4. Ensure a `PlayerInputs` component exists for input (or implement `IVehicleInputProvider`)

## Features

- Modular architecture with clear separation of concerns  
- Realistic multi-stage drivetrain simulation  
- Per-wheel suspension and tire physics  
- Automatic gear shifting with clutch control  
- Configurable via ScriptableObjects  
- Input abstraction for custom controllers  
- Debug visualization and telemetry  

## Status

See [TODO](TODO.md) for current development progress and planned features.
