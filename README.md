# Custom Hardware Monitor

## Features

- **Temperature Monitoring**: Monitor CPU and GPU temperatures
- **Usage Monitoring**: Monitor CPU and GPU usage/load percentages  
- **Multi-Hardware Support**: Automatically detects available hardware components
- **Real-time Updates**: Continuous monitoring with configurable refresh intervals

## How to use

1. Install "Open Hardware Monitor"

    - https://openhardwaremonitor.org/

1. Launch "Open Hardware Monitor"

1. Run TryTemperature.exe

## Available Monitoring Methods

The `HardwareMonitor` class provides these methods:

- `OutputTemperature(string hardwareName)` - Get temperature readings
- `OutputUsage(string hardwareName)` - Get CPU/GPU usage percentages

Both methods return formatted strings with hardware name, sensor name, and current value.

## Example Usage

```csharp
// Get temperature
var temperature = HardwareMonitor.OutputTemperature("Intel Core i7-8700K");
// Output: "Hardware: Intel Core i7-8700K, Name: CPU Package, Sensor value: 45.0"

// Get usage
var usage = HardwareMonitor.OutputUsage("Intel Core i7-8700K");  
// Output: "Hardware: Intel Core i7-8700K, Name: CPU Total, Usage value: 25.5%"
```

## Tips

- Cannot be used if "Open Hardware Monitor" is not running.
- The TryTemperature application now displays both temperature and usage data for selected hardware components.
- WinForms applications will show both temperature and usage readings for all checked hardware components.
