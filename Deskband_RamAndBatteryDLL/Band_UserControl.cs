using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Management;
using System.Drawing;

namespace Deskband_RamAndBatteryDLL
{
    public partial class Band_UserControl: UserControl
    {
        public Band_UserControl(CSDeskBand.CSDeskBandWin w)
        {
            InitializeComponent();
            Timer timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1000; 
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            //Battery Percentage
            PowerStatus powerStatus = SystemInformation.PowerStatus;
            string batteryPercentage = (powerStatus.BatteryLifePercent * 100).ToString();
            bool IsOnline = (SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online);
            label3.Text = batteryPercentage + "%";
            label5.Visible = IsOnline ? true : false;
            // label5.Location = IsOnline ? new Point((label3.Right - label5.Width) + 0, (label3.Top - 0)) : label5.Location;
            label5.Location = IsOnline ? new Point(label3.Right, (label3.Top + 2)) : label5.Location;

            //RAM usage percentage
            var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new {
                FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
            }).FirstOrDefault();

            if (memoryValues != null)
            {
                label4.Text = Math.Round(((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100, 0) + "%";
            }

            
        }
    }
}
