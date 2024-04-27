using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OHMService
{
    public class HardwareMonitorHelper
    {
        public static bool CheckHardwareMonitorAvailable()
        {
            var process = Process.GetProcessesByName("OpenHardwareMonitor").FirstOrDefault();
            return process != null;
        }
    }
}
