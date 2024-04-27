using HardwareMonitor.Models;
using Iot.Device.HardwareMonitor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace HardwareMonitor
{
    public class HardwareMonitor
    {
        public static string OutputTemprature(string hardwareName)
        {
            var identifier = HardwareInfo.GetIdentifier(hardwareName);

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
