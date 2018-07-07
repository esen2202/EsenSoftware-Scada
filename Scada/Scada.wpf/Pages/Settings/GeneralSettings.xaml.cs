using Scada.core;
using Scada.wpf.Pages.Windows;
using Scada.wpf.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scada.wpf.Pages.Settings
{
    /// <summary>
    /// Interaction logic for GeneralSettings.xaml
    /// </summary>
    public partial class GeneralSettings : UserControl
    {

        #region [Definations]
        public delegate void ChangedSettingsHandler();
        public event ChangedSettingsHandler OnChangedGeneralSettings;
        public event ChangedSettingsHandler OnChangedInterfaceSettings;

        //- NotificationPanel newNotify;
        NotificationPanelCall notifyCall;
        #endregion

        public GeneralSettings()
        {
            InitializeComponent();
            GetGeneralSettings();
            GetInterfaceSettings();
        }

        private void Btn_SaveSetGeneral_Click(object sender, RoutedEventArgs e)
        {
            SetGeneralSettings();
        }

        private void GetGeneralSettings()
        {
            Cb_StartWithWindows.IsChecked = Properties.SetGeneral.Default.AppStartWithWindows;
            Cb_AutoConnect.IsChecked = SetGeneral.Default.AutoReconnectToPLC;
            Txt_AutoConnectPeriod.Text = SetGeneral.Default.AutoReconnectPeriod.ToString();
            Cb_AutoDBRecord.IsChecked = SetGeneral.Default.AutoRecordWhenAppStart;
            Txt_AutoDBRecord.Text = SetGeneral.Default.AutoRecordPeriod.ToString();
        }

        private void SetGeneralSettings()
        {
            SetGeneral.Default.AppStartWithWindows = (bool)Cb_StartWithWindows.IsChecked;
            SetGeneral.Default.AutoReconnectToPLC = (bool)Cb_AutoConnect.IsChecked;
            SetGeneral.Default.AutoReconnectPeriod = TypeControls.CheckStrToInt(Txt_AutoConnectPeriod.Text, 10);
            SetGeneral.Default.AutoRecordWhenAppStart = (bool)Cb_AutoDBRecord.IsChecked;
            SetGeneral.Default.AutoRecordPeriod = TypeControls.CheckStrToInt(Txt_AutoDBRecord.Text, 600);
            SetGeneral.Default.Save();

            OnChangedGeneralSettings?.Invoke(); // Event Trigger
            notifyCall = new NotificationPanelCall("Save Settings", "Successfully saved", StatusColor.Success, 3);
        }

        private void GetInterfaceSettings()
        {
            Cb_AlwaysOnTop.IsChecked = SetInterface.Default.AlwaysOnTop ;
            Cb_PreventMinimized.IsChecked = SetInterface.Default.PreventWindowMinimized;
            Cb_PreventResize.IsChecked = SetInterface.Default.PreventWindowResize;
            Cb_PreventClose.IsChecked = SetInterface.Default.PreventClose;
            Cb_HideIconOnTaskbar.IsChecked = SetInterface.Default.HideIconOnTaskbar;
        }

        private void SetInterfaceSettings()
        {
            SetInterface.Default.AlwaysOnTop = (bool)Cb_AlwaysOnTop.IsChecked;
            SetInterface.Default.PreventWindowMinimized = (bool)Cb_PreventMinimized.IsChecked;
            SetInterface.Default.PreventWindowResize = (bool)Cb_PreventResize.IsChecked;
            SetInterface.Default.PreventClose = (bool)Cb_PreventClose.IsChecked;
            SetInterface.Default.HideIconOnTaskbar = (bool)Cb_HideIconOnTaskbar.IsChecked;
            SetInterface.Default.Save();

            OnChangedInterfaceSettings?.Invoke(); // Event Trigger
            notifyCall = new NotificationPanelCall("Save Settings", "Successfully saved", StatusColor.Success, 3);
        }

        private void Btn_SaveSetInterface_Click(object sender, RoutedEventArgs e)
        {
            SetInterfaceSettings();
        }
    }
}
