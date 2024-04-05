using HardwareMonitor.Models;
using Iot.Device.HardwareMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace HardwareMonitor
{
    public class HardwareMonitor
    {
        private readonly List<HardwareMappingList> HardwareMappingList = [];

        public void RegisterHardwareMappingList()
        {
            var monitor = new OpenHardwareMonitor();

            foreach (var hardware in monitor.GetHardwareComponents())
            {
                HardwareMappingList.Add(new HardwareMappingList()
                {
                    Identifier = hardware.Identifier,
                    HardwareName = hardware.Name
                });
            }
        }

        public void OutputTemprature()
        {
            try
            {
                var monitor = new OpenHardwareMonitor();

                foreach (var sensor in monitor.GetSensorList())
                {
                    if (sensor.SensorType == SensorType.Temperature)
                    {
                        if (sensor.TryGetValue(out IQuantity value))
                        {
                            var hardwareInfo = HardwareMappingList.FirstOrDefault(x => x.Identifier == sensor.Parent);
                            if (hardwareInfo.HardwareName == "NVIDIA NVIDIA GeForce GTX 1070")
                            {
                                Console.WriteLine($"Hardware: {hardwareInfo.HardwareName}, Name: {sensor.Name}, Sensor value: {value.Value}");
                            }
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
        }
    }
}
