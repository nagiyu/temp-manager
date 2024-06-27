using Common;
using OHMService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private bool isMonitoring = false;

        public Form1()
        {
            if (!Utils.IsDebugging())
            {
                Task.Run(() => OpenHardwareMonitor.Program.Main());
            }

            HardwareInfo.RegisterHardwareMappingList();
            InitializeComponent();
        }

        private void Click_GetTempreture(object sender, EventArgs e)
        {
            isMonitoring = true;
        }

        private void Click_Stop(object sender, EventArgs e)
        {
            isMonitoring = false;
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (!isMonitoring)
            {
                return;
            }

            var checkedItems = checkedListBox1.CheckedItems.Cast<string>().ToList();
            foreach (var item in checkedItems)
            {
                textBox1.Text = HardwareMonitor.OutputTemprature(item) + Environment.NewLine + textBox1.Text;
            }
        }

        private void Click_Clear(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void Click_OutputText(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "output.txt";
            saveFileDialog.Filter = "Text File | *.txt";
            saveFileDialog.Title = "Save an Text File";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, textBox1.Text);
            }
        }
    }
}
