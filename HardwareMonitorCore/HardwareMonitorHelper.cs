using System.Diagnostics;
using System.Linq;

namespace OHMService
{
    public class HardwareMonitorHelper
    {
        private const string HardwareMonitorProcessName = "OpenHardwareMonitor";
        
        public static bool CheckHardwareMonitorAvailable()
        {
            return Process.GetProcessesByName(HardwareMonitorProcessName).Any();
        }
    }
}
