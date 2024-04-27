using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread thread1 = new Thread(() =>
            {
                OpenHardwareMonitor.Program.Main();
            });

            Thread thread2 = new Thread(() =>
            {
                Thread.Sleep(3000);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            });

            thread1.Start();
            thread2.Start();
        }
    }
}
