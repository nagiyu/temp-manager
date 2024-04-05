namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private List<HardwareMonitor.Models.HardwareMappingList> hardwareMappingList = [];

        private bool isMonitoring = false;

        public Form1()
        {
            var monitor = new HardwareMonitor.HardwareMonitor();
            hardwareMappingList = monitor.RegisterHardwareMappingList();

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
            var monitor = new HardwareMonitor.HardwareMonitor();
            foreach (var item in checkedItems)
            {
                textBox1.Text = monitor.OutputTemprature(hardwareMappingList, item) + Environment.NewLine + textBox1.Text;
            }
        }

        private void Click_Clear(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
