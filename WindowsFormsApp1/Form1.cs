using Common;
using OHMService;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // Constants for file dialogs
        private const string DefaultTextFileName = "output.txt";
        private const string DefaultExcelFileName = "output.xlsx";
        private const string TextFileFilter = "Text File | *.txt";
        private const string ExcelFileFilter = "Excel File | *.xlsx";
        private const string SaveTextFileTitle = "Save a Text File";
        private const string SaveExcelFileTitle = "Save an Excel File";
        
        // Excel headers
        private const string HeaderName = "Name";
        private const string HeaderSensor = "Sensor";
        private const string HeaderValue = "Value";
        
        // Log file constants
        private const string DefaultLogFileName = "monitoring_log.txt";
        private const string LogFileFilter = "Log File | *.txt";
        private const string SelectLogFileTitle = "Select Log File Path";
        
        private bool isMonitoring = false;
        private bool isLoggingEnabled = false;
        private string logFilePath = string.Empty;

        public Form1()
        {
            if (!Utils.IsDebugging())
            {
                Task.Run(() => OpenHardwareMonitor.Program.Main());
            }

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

        private static SaveFileDialog CreateSaveFileDialog(string fileName, string filter, string title)
        {
            return new SaveFileDialog
            {
                FileName = fileName,
                Filter = filter,
                Title = title
            };
        }

        private void Click_Clear(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void Click_OutputText(object sender, EventArgs e)
        {
            var saveFileDialog = CreateSaveFileDialog(DefaultTextFileName, TextFileFilter, SaveTextFileTitle);
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, textBox1.Text);
            }
        }

        private static void WriteExcelHeaders(ClosedXML.Excel.IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = HeaderName;
            worksheet.Cell("B1").Value = HeaderSensor;
            worksheet.Cell("C1").Value = HeaderValue;
        }

        private static void WriteExcelData(ClosedXML.Excel.IXLWorksheet worksheet, string textData)
        {
            var lines = textData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Split(',');
                for (int j = 0; j < line.Length && j < 3; j++) // Limit to 3 columns
                {
                    var cellValue = line[j].Split(':').LastOrDefault()?.Trim() ?? string.Empty;
                    worksheet.Cell(i + 2, j + 1).Value = cellValue;
                }
            }
        }

        private void Click_OutputExcel(object sender, EventArgs e)
        {
            var saveFileDialog = CreateSaveFileDialog(DefaultExcelFileName, ExcelFileFilter, SaveExcelFileTitle);
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var workbook = new ClosedXML.Excel.XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Sheet1");
                
                WriteExcelHeaders(worksheet);
                WriteExcelData(worksheet, textBox1.Text);
                
                workbook.SaveAs(saveFileDialog.FileName);
            }
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
                var saveFileDialog = CreateSaveFileDialog(DefaultLogFileName, LogFileFilter, SelectLogFileTitle);
                
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
