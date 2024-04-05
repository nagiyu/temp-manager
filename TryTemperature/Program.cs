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

        monitor.RegisterHardwareMappingList();

        while (!Console.KeyAvailable)
        {
            monitor.OutputTemprature();
            Thread.Sleep(1000);
        }
        Console.ReadKey();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
