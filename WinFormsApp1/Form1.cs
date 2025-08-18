using OHMService;
using OHMService.Models;
using System.Linq;
using System.Text;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Log file constants
        private const string DefaultLogFileName = "monitoring_log.txt";
        private const string LogFileFilter = "Log File | *.txt";
        private const string SelectLogFileTitle = "Select Log File Path";
        
        private List<HardwareMappingList> hardwareMappingList = [];

        private bool isMonitoring = false;
        private bool isLoggingEnabled = false;
        private string logFilePath = string.Empty;

        public Form1()
        {
            HardwareInfo.RegisterHardwareMappingList();
            InitializeComponent();
        }

        private void Click_GetTemperature(object sender, EventArgs e)
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

            var checkedItems = checkedListBox1.CheckedItems.Cast<string>();
            var temperatureEntries = checkedItems
                .Select(item => HardwareMonitor.OutputTemperature(item))
                .Where(entry => !string.IsNullOrEmpty(entry));
            
            var usageEntries = checkedItems
                .Select(item => HardwareMonitor.OutputUsage(item))
                .Where(entry => !string.IsNullOrEmpty(entry));

            var allEntries = temperatureEntries.Concat(usageEntries);

            if (allEntries.Any())
            {
                var stringBuilder = new StringBuilder();
                foreach (var entry in allEntries)
                {
                    stringBuilder.AppendLine(entry);
                }
                
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    stringBuilder.Append(textBox1.Text);
                }
                
                textBox1.Text = stringBuilder.ToString();
                
                // Write to log file if logging is enabled
                if (isLoggingEnabled && !string.IsNullOrEmpty(logFilePath))
                {
                    WriteToLogFile(allEntries);
                }
            }
        }

        private void Click_Clear(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void WriteToLogFile(IEnumerable<string> entries)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var logEntries = entries.Select(entry => $"[{timestamp}] {entry}");
                System.IO.File.AppendAllLines(logFilePath, logEntries);
            }
            catch (Exception ex)
            {
                // Silently handle file write errors to avoid interrupting monitoring
                // In a production app, you might want to log this error somewhere else
            }
        }

        private void CheckBoxLogging_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null) return;

            if (checkBox.Checked)
            {
                // Enable logging - show file dialog to select log file path
                var saveFileDialog = new SaveFileDialog
                {
                    FileName = DefaultLogFileName,
                    Filter = LogFileFilter,
                    Title = SelectLogFileTitle
                };
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    logFilePath = saveFileDialog.FileName;
                    isLoggingEnabled = true;
                    
                    // Write header to log file
                    try
                    {
                        var header = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Monitoring log started";
                        System.IO.File.AppendAllLines(logFilePath, new[] { header, "" });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to create log file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        checkBox.Checked = false;
                        isLoggingEnabled = false;
                        logFilePath = string.Empty;
                    }
                }
                else
                {
                    // User cancelled file dialog
                    checkBox.Checked = false;
                    isLoggingEnabled = false;
                    logFilePath = string.Empty;
                }
            }
            else
            {
                // Disable logging
                if (isLoggingEnabled && !string.IsNullOrEmpty(logFilePath))
                {
                    try
                    {
                        var footer = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Monitoring log ended";
                        System.IO.File.AppendAllLines(logFilePath, new[] { footer, "" });
                    }
                    catch (Exception ex)
                    {
                        // Silently handle write errors when stopping
                    }
                }
                
                isLoggingEnabled = false;
                logFilePath = string.Empty;
            }
        }
    }
}
