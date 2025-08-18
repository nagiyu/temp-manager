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
            var newEntries = checkedItems
                .Select(item => HardwareMonitor.OutputTemperature(item))
                .Where(entry => !string.IsNullOrEmpty(entry));

            if (newEntries.Any())
            {
                var stringBuilder = new StringBuilder();
                foreach (var entry in newEntries)
                {
                    stringBuilder.AppendLine(entry);
                }
                
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    stringBuilder.Append(textBox1.Text);
                }
                
                textBox1.Text = stringBuilder.ToString();
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
    }
}
