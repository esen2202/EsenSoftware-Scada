using Scada.core;
using Scada.wpf.Pages.Windows;
using Scada.wpf.Properties;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Scada.wpf.Pages.Comm.Profinet
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        //- NotificationPanel newNotify;
        NotificationPanelCall notifyCall;
        public Settings()
        {
            InitializeComponent();
            GetSettings();
        }

        #region .settings file operations

        // Get Settings
        private void GetSettings()
        {
            txt_DeviceName.Text = SetPLC.Default.Profinet_DeviceName;
            txt_CPURock.Text = SetPLC.Default.Profinet_CpuRock.ToString();
            txt_Slot.Text = SetPLC.Default.Profinet_Slot.ToString();
            txt_IpAddress.Text = SetPLC.Default.Profinet_IP;
            txt_TimeOut.Text = SetPLC.Default.Profinet_TimeOut.ToString();
            txt_Interval.Text = SetPLC.Default.Profinet_Interval.ToString();
        }

        // Save Settings
        private void SaveSettings()
        {

            if (IPAddress.TryParse(txt_IpAddress.Text, out IPAddress ip)) { /* Ip address kontrolü */ }

            SetPLC.Default.Profinet_DeviceName = txt_DeviceName.Text;
            SetPLC.Default.Profinet_CpuRock = Convert.ToInt16(txt_CPURock.Text);
            SetPLC.Default.Profinet_Slot = Convert.ToInt16(txt_Slot.Text);
            SetPLC.Default.Profinet_IP = txt_IpAddress.Text;
            SetPLC.Default.Profinet_TimeOut = Convert.ToInt32(txt_TimeOut.Text);
            SetPLC.Default.Profinet_Interval = Convert.ToInt16(txt_Interval.Text);
            SetPLC.Default.Save();

            notifyCall = new NotificationPanelCall("Save Settings", "Successfully saved", StatusColor.Success, 3);
        }

        #endregion

        private void btn_SaveCommSettings_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.DisconnectDevice();
            SaveSettings();
            mainWindow.AssignConnection();

        }

    }
}
