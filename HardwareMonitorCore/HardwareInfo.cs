using Iot.Device.HardwareMonitor;
using OHMService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OHMService
{
    public class HardwareInfo
    {
        private static List<HardwareMappingList> hardwareMappingList = [];

        public static void RegisterHardwareMappingList()
        {
            hardwareMappingList = HardwareMonitor.GetHardwareComponents();
        }

        public static List<string> GetHardwareNameList()
        {
            return hardwareMappingList.Select(x => x.HardwareName).ToList();
        }

        public static string GetIdentifier(string hardwareName)
        {
            return hardwareMappingList.FirstOrDefault(x => x.HardwareName == hardwareName).Identifier;
        }
    }
}
