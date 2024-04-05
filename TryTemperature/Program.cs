using HardwareMonitor;

class Program
{
    static void Main()
    {
        if (!HardwareMonitorHelper.CheckHardwareMonitorAvailable())
        {
            Console.WriteLine("Error: OpenHardwareMonitor is not running.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            return;
        }

        var monitor = new HardwareMonitor.HardwareMonitor();

        var hardwareMappingList = monitor.RegisterHardwareMappingList();

        while (!Console.KeyAvailable)
        {
            Console.WriteLine(monitor.OutputTemprature(hardwareMappingList, "NVIDIA NVIDIA GeForce GTX 1070"));
            Thread.Sleep(1000);
        }
        Console.ReadKey();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
