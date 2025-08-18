using OHMService;

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

        HardwareInfo.RegisterHardwareMappingList();

        while (!Console.KeyAvailable)
        {
            Console.WriteLine("=== Temperature ===");
            Console.WriteLine(HardwareMonitor.OutputTemperature("NVIDIA NVIDIA GeForce GTX 1070"));
            Console.WriteLine("=== Usage ===");
            Console.WriteLine(HardwareMonitor.OutputUsage("NVIDIA NVIDIA GeForce GTX 1070"));
            Console.WriteLine();
            Thread.Sleep(1000);
        }
        Console.ReadKey();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
