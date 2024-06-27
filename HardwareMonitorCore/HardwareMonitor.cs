using OHMService.Models;
using Iot.Device.HardwareMonitor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using Common;

namespace OHMService
{
    public class HardwareMonitor
    {
        public static List<HardwareMappingList> GetHardwareComponents()
        {
            if (Utils.IsDebugging())
            {
                return new List<HardwareMappingList>()
                {
                    new HardwareMappingList()
                    {
                        Identifier = "CPU",
                        HardwareName = "Intel Core i7-8700K"
                    },
                    new HardwareMappingList()
                    {
                        Identifier = "GPU",
                        HardwareName = "NVIDIA GeForce RTX 2080 Ti"
                    }
                };
            }

            var monitor = new OpenHardwareMonitor();
            var hardwareMappingList = new List<HardwareMappingList>();
            foreach (var hardware in monitor.GetHardwareComponents())
            {
                hardwareMappingList.Add(new HardwareMappingList()
                {
                    Identifier = hardware.Identifier,
                    HardwareName = hardware.Name
                });
            }

            return hardwareMappingList;
        }

        public static string OutputTemprature(string hardwareName)
        {
            var identifier = HardwareInfo.GetIdentifier(hardwareName);

            if (Utils.IsDebugging())
            {
                return $"Hardware: {hardwareName}, Name: {"Sensor"}, Sensor value: {"hogehoge"}";
            }

            try
            {
                var monitor = new OpenHardwareMonitor();

                foreach (var sensor in monitor.GetSensorList())
                {
                    if (sensor.SensorType != SensorType.Temperature)
                    {
                        continue;
                    }

                    if (sensor.Parent != identifier)
                    {
                        continue;
                    }

                    if (sensor.TryGetValue(out IQuantity value))
                    {
                        return $"Hardware: {hardwareName}, Name: {sensor.Name}, Sensor value: {value.Value}";
                    }
                    else
                    {
                        return "Value could not be retrieved.";
                    }
                }

                return "Temperature sensor not found.";
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }
    }
}
