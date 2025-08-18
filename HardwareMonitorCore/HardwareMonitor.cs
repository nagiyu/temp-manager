using OHMService.Models;
using Iot.Device.HardwareMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitsNet;
using Common;

namespace OHMService
{
    public class HardwareMonitor
    {
        // Constants for error messages and debug values
        private const string DebugSensorName = "Sensor";
        private const string DebugSensorValue = "hogehoge";
        private const string ErrorValueNotRetrieved = "Value could not be retrieved.";
        private const string ErrorTemperatureSensorNotFound = "Temperature sensor not found.";
        private const string ErrorUsageSensorNotFound = "Usage sensor not found.";
        private const string ErrorPrefix = "Error: ";
        
        public static List<HardwareMappingList> GetHardwareComponents()
        {
            if (Utils.IsDebugging())
            {
                return GetDebugHardwareComponents();
            }

            return GetRealHardwareComponents();
        }

        private static List<HardwareMappingList> GetDebugHardwareComponents()
        {
            return new List<HardwareMappingList>
            {
                new HardwareMappingList
                {
                    Identifier = "CPU",
                    HardwareName = "Intel Core i7-8700K"
                },
                new HardwareMappingList
                {
                    Identifier = "GPU",
                    HardwareName = "NVIDIA GeForce RTX 2080 Ti"
                }
            };
        }

        private static List<HardwareMappingList> GetRealHardwareComponents()
        {
            var monitor = new OpenHardwareMonitor();
            return monitor.GetHardwareComponents()
                .Select(hardware => new HardwareMappingList
                {
                    Identifier = hardware.Identifier,
                    HardwareName = hardware.Name
                })
                .ToList();
        }

        public static string OutputTemperature(string hardwareName)
        {
            var identifier = HardwareInfo.GetIdentifier(hardwareName);

            if (Utils.IsDebugging())
            {
                return $"Hardware: {hardwareName}, Name: {DebugSensorName}, Sensor value: {DebugSensorValue}";
            }

            if (string.IsNullOrEmpty(identifier))
            {
                return ErrorTemperatureSensorNotFound;
            }

            try
            {
                var monitor = new OpenHardwareMonitor();
                
                var temperatureSensor = monitor.GetSensorList()
                    .Where(sensor => sensor.SensorType == SensorType.Temperature)
                    .FirstOrDefault(sensor => sensor.Parent == identifier);

                if (temperatureSensor == null)
                {
                    return ErrorTemperatureSensorNotFound;
                }

                if (temperatureSensor.TryGetValue(out IQuantity value))
                {
                    return $"Hardware: {hardwareName}, Name: {temperatureSensor.Name}, Sensor value: {value.Value}";
                }

                return ErrorValueNotRetrieved;
            }
            catch (Exception e)
            {
                return ErrorPrefix + e.Message;
            }
        }

        public static string OutputUsage(string hardwareName)
        {
            var identifier = HardwareInfo.GetIdentifier(hardwareName);

            if (Utils.IsDebugging())
            {
                return $"Hardware: {hardwareName}, Name: {DebugSensorName}, Usage value: {DebugSensorValue}%";
            }

            if (string.IsNullOrEmpty(identifier))
            {
                return ErrorUsageSensorNotFound;
            }

            try
            {
                var monitor = new OpenHardwareMonitor();
                
                var usageSensor = monitor.GetSensorList()
                    .Where(sensor => sensor.SensorType == SensorType.Load)
                    .FirstOrDefault(sensor => sensor.Parent == identifier);

                if (usageSensor == null)
                {
                    return ErrorUsageSensorNotFound;
                }

                if (usageSensor.TryGetValue(out IQuantity value))
                {
                    return $"Hardware: {hardwareName}, Name: {usageSensor.Name}, Usage value: {value.Value}%";
                }

                return ErrorValueNotRetrieved;
            }
            catch (Exception e)
            {
                return ErrorPrefix + e.Message;
            }
        }
    }
}
