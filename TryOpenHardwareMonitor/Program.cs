using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryOpenHardwareMonitor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var computer = new Computer
            {
                CPUEnabled = true,
                GPUEnabled = true
            };

            computer.Open();

            foreach (var hardware in computer.Hardware)
            {
                hardware.Update();
                Console.WriteLine("Hardware: " + hardware.Name);
                foreach (var sensor in hardware.Sensors)
                {
                    Console.WriteLine("Type: " + sensor.SensorType + " Sensor: " + sensor.Name + " Value: " + sensor.Value);
                }
            }

            computer.Close();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
