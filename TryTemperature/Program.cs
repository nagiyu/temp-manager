using Iot.Device.HardwareMonitor;
using UnitsNet;

class Program
{
    public class HardwareMappingList
    {
        public string Identifier { get; set; }

        public string HardwareName { get; set; }
    }

    static void Main()
    {
        var hardwareMappingList = new List<HardwareMappingList>();

        try
        {
            var monitor = new OpenHardwareMonitor();

            foreach (var hardware in monitor.GetHardwareComponents())
            {
                hardwareMappingList.Add(new HardwareMappingList() { 
                    Identifier = hardware.Identifier, 
                    HardwareName = hardware.Name 
                });
            }

            foreach (var sensor in monitor.GetSensorList())
            {
                if (sensor.SensorType == SensorType.Temperature) 
                {
                    if (sensor.TryGetValue(out IQuantity value))
                    {
                        var hardwareInfo = hardwareMappingList.FirstOrDefault(x => x.Identifier == sensor.Parent);
                        Console.WriteLine($"Hardware: {hardwareInfo.HardwareName}, Name: {sensor.Name}, Sensor value: {value}");
                    }
                    else
                    {
                        Console.WriteLine("Value could not be retrieved.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
