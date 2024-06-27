using System;
using System.Diagnostics;

namespace Common
{
    public class Utils
    {
        /// <summary>
        /// Check if the application is running in debug mode.
        /// </summary>
        /// <returns>True if the application is running in debug mode, false otherwise.</returns>
        public static bool IsDebugging()
        {
            return Debugger.IsAttached;
        }

    }
}
