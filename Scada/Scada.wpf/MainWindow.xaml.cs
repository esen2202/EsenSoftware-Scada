#define Diper1

using Scada.core;
using Scada.model;
using Scada.wpf.Classes;
using Scada.wpf.Classes.User;
using Scada.wpf.Pages.Comm;
using Scada.wpf.Pages.Settings;
using Scada.wpf.Pages.UserControls;
using Scada.wpf.Pages.UserControls.Information;
using Scada.wpf.Pages.UserControls.UserManagement;
using Scada.wpf.Pages.Windows;
using Scada.wpf.Properties;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


#if Diper
using Scada.Diper.Reporting;
using Scada.Diper.Communication;
using Scada.Diper.Information;
using Scada.Diper.Monitoring;
using Scada.Diper.Classes;
using Scada.Diper.Settings;

#else

#endif

namespace Scada.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IUserInfoTransfer
    {
        //! General Definations
        #region [General Definations]

        public static DataContextBundle dataContextMainWindow;
        
        private DispatcherTimer saat = new System.Windows.Threading.DispatcherTimer();

        GridLength gl80 = new GridLength(80, GridUnitType.Pixel);
        GridLength gl250 = new GridLength(250, GridUnitType.Pixel);

        //- NotificationPanel newNotify;
        NotificationPanelCall notifyCall;

        UserEdit createUserUC;
        Login loginUC;
        CommSettings commSetUC;

        #endregion

        //! Window Structures
        #region [Window Structures]

        private void InitializeStatus()
        {
            dp_UserInfo.Visibility = (gc_sideMenu.Width == gl80) ? Visibility.Visible : Visibility.Hidden;
            img_LoginBtn.IsEnabled = !(dataContextMainWindow.UserInfo.Authorization > 0);
            img_LogoutBtn.IsEnabled = (dataContextMainWindow.UserInfo.Authorization > 0);
            img_CommSettings.IsEnabled = (dataContextMainWindow.UserInfo.Authorization == 3);
            btn_settings.IsEnabled = (dataContextMainWindow.UserInfo.Authorization == 3);
            mbtn_settings.IsEnabled = (dataContextMainWindow.UserInfo.Authorization > 1);

            img_UserManagementBtn.IsEnabled = (dataContextMainWindow.UserInfo.Authorization > 0);
            lbl_Authorized.Visibility = (dataContextMainWindow.UserInfo.Authorization > 0) ? Visibility.Visible : Visibility.Collapsed;

            this.btn_min.Visibility = SetInterface.Default.PreventWindowMinimized ? Visibility.Collapsed : Visibility.Visible;
            this.btn_max.Visibility = SetInterface.Default.PreventWindowResize ? Visibility.Collapsed : Visibility.Visible;
            this.btn_close.Visibility = SetInterface.Default.PreventClose ? Visibility.Collapsed : Visibility.Visible;
            this.ShowInTaskbar = SetInterface.Default.HideIconOnTaskbar ? false : true;

            this.ResizeMode = SetInterface.Default.PreventWindowResize ? ResizeMode.CanResize : ResizeMode.CanResizeWithGrip;
            this.Topmost = SetInterface.Default.AlwaysOnTop ? true : false;

            this.WindowState = (!SetInterface.Default.WindowMaximized && !SetInterface.Default.PreventWindowResize) ? WindowState.Normal : WindowState.Maximized;

            gc_sideMenu.Width = (SetInterface.Default.SideBarSymbolic) ? gl80 : gl250;
            dp_UserInfo.Visibility = (SetInterface.Default.SideBarSymbolic) ? Visibility.Visible : Visibility.Hidden;

        }

        public MainWindow()
        {
            // FrameworkElement.LanguageProperty.OverrideMetadata(
            //typeof(FrameworkElement),
            //new FrameworkPropertyMetadata(
            //    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

            InitializeComponent();

            #region RegistryControls 
            if (SetGeneral.Default.AppStartWithWindows)
            {
                core.Custom.RegistryOperations.SetStartup(System.Reflection.Assembly.GetEntryAssembly().Location);

            }
            else
            {
                core.Custom.RegistryOperations.DeleteStartup();
            }

            #endregion

            #region Real Time Clock

            saat.Interval = TimeSpan.FromSeconds(1);
            saat.IsEnabled = true;
            saat.Tick += Timer_Tick;

            #endregion

            #region Window Properties

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            #endregion

            #region DataContext

            dataContextMainWindow = new DataContextBundle { UserInfo = UserInfo.InvalidUser };
            this.DataContext = dataContextMainWindow;

            #endregion

            InitializeStatus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AssignConnection();

            Mbtn_monitoring_Click(mbtn_monitoring, e);
            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (createUserUC != null)
                Canvas.SetLeft(createUserUC, (double)((cv_ContentUC.ActualWidth - createUserUC.Width) / 2));
            if (commSetUC != null)
                Canvas.SetLeft(commSetUC, (double)((cv_ContentUC.ActualWidth - commSetUC.Width) / 2));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SetInterface.Default.PreventClose)
            {
                e.Cancel = true;
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Exit from Scada", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            // save .settings file
            SetInterface.Default.Save();

            // close all windows 
            var windows = Application.Current.Windows;
            foreach (var item in windows)
            {
                if ((item as Window).Title.ToLower() == "mainwindow") continue;
                (item as Window).Close();
            }
            Environment.Exit(0);

        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            //switch (this.WindowState)
            //{
            //    case WindowState.Maximized:
            //        this.WindowState = WindowState.Maximized;
            //        break;
            //    case WindowState.Minimized:
            //        this.WindowState = SetInterface.Default.PreventWindowMinimized ? this.WindowState: WindowState.Minimized;
            //        break;
            //    case WindowState.Normal:
            //        this.WindowState = SetInterface.Default.PreventWindowResize ? this.WindowState : WindowState.Normal;
            //        break;
            //}
        }

        #endregion

        //! Communication
        #region [Communication]

        /// <summary>
        /// Assign connection object, then connect
        /// </summary>
        public void AssignConnection()
        {  
            ConnectToDevice();
        }

        /// <summary>
        /// CommInterface -> Connect()
        /// </summary>
        public void ConnectToDevice()
        {
            Thread.Sleep(200);

        }

        public void DisconnectDevice()
        {
            Thread.Sleep(200);
   
        }

        /// <summary>
        /// Set view as connection status 
        /// </summary>
        /// <param name="connected">is connected</param>
        private void ConnectionStatus(bool connected)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (connected)
                {
                    img_ConnectionStatus.Source = new BitmapImage(new Uri(@"pack://application:,,,/Scada.wpf;component/" + "Assets/img/icons/g-Checkmark-96.png", UriKind.Absolute));
                    img_ToolBarConnectionStatus.Source = new BitmapImage(new Uri(@"pack://application:,,,/Scada.wpf;component/" + "Assets/img/icons/b-Link-50.png", UriKind.Absolute));
                    Lbl_PlcStatus.Content = "Connected";
                    img_ReConnectToPLC.IsEnabled = false;
                    notifyCall = new NotificationPanelCall("Connection", "Connected the device", StatusColor.Success, 3);
                }
                else
                {
                    img_ConnectionStatus.Source = new BitmapImage(new Uri(@"pack://application:,,,/Scada.wpf;component/" + "Assets/img/icons/r-Multiply-96.png", UriKind.Absolute));
                    img_ToolBarConnectionStatus.Source = new BitmapImage(new Uri(@"pack://application:,,,/Scada.wpf;component/" + "Assets/img/icons/b-Broken Link-64.png", UriKind.Absolute));
                    Lbl_PlcStatus.Content = "No Connection";
                    img_ReConnectToPLC.IsEnabled = true;
                }

                img_ConnectionStatus2.Source = img_ConnectionStatus.Source;
                img_ConnectionStatus3.Source = img_ConnectionStatus.Source;
                Lbl_PlcAddress.Content = SetPLC.Default.Profinet_IP;
                Lbl_PlcName.Content = SetPLC.Default.Profinet_DeviceName;
                Lbl_PlcName2.Content = Lbl_PlcName.Content;
                Lbl_PlcStatus2.Content = Lbl_PlcStatus.Content;
                Lbl_PlcAddress2.Content = Lbl_PlcAddress.Content;

            });

        }

        //! Event Methods
        #region [Event Methods]

        /// <summary>
        /// from CommInterface
        /// </summary>
        private void PlcCommunication_OnCouldNotConnect()
        {
            ConnectionStatus(false);
            notifyCall = new NotificationPanelCall("Connection Failed", "Could not connect the device", StatusColor.Error, 3);
        }

        /// <summary>
        /// from CommInterface 
        /// </summary>
        private void PlcCommunication_OnConnected()
        {
            ConnectionStatus(true);
        }

        private void PlcCommunication_OnWhileConnecting()
        {
            // throw new NotImplementedException();
            // MainAdapter.TimerPause(); //-comment
        }

        /// <summary>
        /// from CommInterface
        /// </summary>
        private void PlcCommunication_OnDisconnected()
        {
            ConnectionStatus(false);
            notifyCall = new NotificationPanelCall("Connection Failed", "Device is disconnected", StatusColor.Error, 3);
        }

        /// <summary>
        /// from CommInterface
        /// </summary>
        private void PlcCommunication_OnReadedData()
        {
            // throw new NotImplementedException();
        }

        /// <summary>
        /// from CommInterface
        /// </summary>
        private void PlcCommunication_OnChangedConnection()
        {
            // throw new NotImplementedException();
        }

        #endregion

        #endregion

        //! Window Controls
        #region [Window Controls]

        private static int ReconnectCounter;
        /// <summary>
        /// Timer / 1sec : Real Time Clock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            lb_time.Content = now.ToLongTimeString();
            lb_date.Content = now.ToLongDateString();

            if (SetGeneral.Default.AutoReconnectToPLC)
            {
                if (Lbl_ReconCounter.Visibility != Visibility.Visible) Lbl_ReconCounter.Visibility = Visibility.Visible;
                Lbl_ReconCounter.Content = SetGeneral.Default.AutoReconnectPeriod - ReconnectCounter;
                ReconnectCounter++;
                if (SetGeneral.Default.AutoReconnectPeriod < ReconnectCounter)
                {
                    ConnectToDevice();
                    ReconnectCounter = 0;
                }
            }
            else
            {
                if (Lbl_ReconCounter.Visibility != Visibility.Hidden) Lbl_ReconCounter.Visibility = Visibility.Hidden;
                ReconnectCounter = 0;
            }
        }

        /// <summary>
        /// Window Control : Close Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            //Environment.Exit(0);
            this.Close();
        }

        /// <summary>
        /// Title Bar Controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bd_titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (e.ClickCount == 2)
            {
                Btn_max_Click(sender, e);
            }
        }

        /// <summary>
        /// Title Bar Control Button : Maximized <-> Normal 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_max_Click(object sender, RoutedEventArgs e)
        {
            if (!SetInterface.Default.PreventWindowResize)
            {
                this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
                SetInterface.Default.WindowMaximized = (this.WindowState == WindowState.Maximized) ? true : false;
            }
        }


        /// <summary>
        /// Title Bar Control Button : Minimized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Left Side Menu - Toggle Symbolic / with Text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_extendMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gc_sideMenu.Width = (gc_sideMenu.Width == gl80) ? gl250 : gl80;
            dp_UserInfo.Visibility = (gc_sideMenu.Width == gl80) ? Visibility.Visible : Visibility.Hidden;
            SetInterface.Default.SideBarSymbolic = (gc_sideMenu.Width == gl80) ? true : false;

        }

        /// <summary>
        /// Action -> ConnectToDevice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_ReConnectToPLC_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConnectToDevice();
        }

        /// <summary>
        /// Call UserControl CommSettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_CommSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cv_ContentUC.Children.Clear();
            commSetUC = new CommSettings();
            Canvas.SetTop(commSetUC, 98);
            Canvas.SetLeft(commSetUC, (cv_ContentUC.ActualWidth - commSetUC.Width) / 2);
            core.UserControlOperations.ContentUserControl(cv_ContentUC, commSetUC);

        }

        #endregion

        //! Paging
        #region [Paging]

        /// <summary>
        /// Menu Button : Open Reporting UC
        /// Menu Button : Open Reporting UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mbtn_reporting_Click(object sender, RoutedEventArgs e)
        {
            ReportsUC reportsUC = new ReportsUC();
            
            core.UserControlOperations.ContentUserControl(dp_ContentUC, reportsUC);
        }

        private void Mbtn_monitoring_Click(object sender, RoutedEventArgs e)
        {
            MonitoringUC monitoringUC = new MonitoringUC();

            core.UserControlOperations.ContentUserControl(dp_ContentUC, monitoringUC);
        }

        private void Mbtn_settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsUC settingsUC = new SettingsUC();

            core.UserControlOperations.ContentUserControl(dp_ContentUC, settingsUC);
        }

        private void mbtn_manual_Click(object sender, RoutedEventArgs e)
        {
            ServiceUC serviceUC = new ServiceUC();

            core.UserControlOperations.ContentUserControl(dp_ContentUC, serviceUC);
        }

        #region Settings Events - Apply New Values On Changed Settings 

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            GeneralSettings generalSettings = new GeneralSettings();
            generalSettings.OnChangedGeneralSettings += GeneralSettings_OnChangedGeneralSettings;
            generalSettings.OnChangedInterfaceSettings += GeneralSettings_OnChangedInterfaceSettings;
            core.UserControlOperations.ContentUserControl(dp_ContentUC, generalSettings);
        }

        private void GeneralSettings_OnChangedInterfaceSettings()
        {

        }

        private void GeneralSettings_OnChangedGeneralSettings()
        {

        }
        #endregion


        /// <summary>
        /// Interface Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_IntefaceSettings_Click(object sender, RoutedEventArgs e)
        {
            core.UserControlOperations.ContentUserControl(dp_ContentUC, new GeneralSettings());
        }

        /// <summary>
        /// -? Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_help_Click(object sender, RoutedEventArgs e)
        {
            About aboutUC = new About();
            Canvas.SetTop(aboutUC, 98);
            Canvas.SetLeft(aboutUC, (cv_ContentUC.ActualWidth - (aboutUC.Width + 20)));
            core.UserControlOperations.ContentUserControl(cv_ContentUC, aboutUC, false);
        }

        /// <summary>
        /// Menu Buttons : Open Edit User UC 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_editUserShow_Click(object sender, RoutedEventArgs e)
        {
            //core.UserControlOperations.ContentUserControl(sp_ContentUC, new UserEdit());
        }

        /// <summary>
        /// Menu Button : Open Login UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_LoginBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cv_ContentUC.Children.Clear();
            loginUC = new Login();
            Canvas.SetTop(loginUC, 98);
            Canvas.SetLeft(loginUC, (cv_ContentUC.ActualWidth - loginUC.Width) / 2);
            core.UserControlOperations.ContentUserControl(cv_ContentUC, loginUC);
        }

        /// <summary>
        /// Menu Button : Logout Action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_LogoutBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (dataContextMainWindow.UserInfo.Authorization > 0)
            {
                dataContextMainWindow.UserInfo = UserInfo.InvalidUser;

                notifyCall = new NotificationPanelCall("Logout", "Successful", StatusColor.Success, 3);
            }

            cv_ContentUC.Children.Clear();
            createUserUC = null;

            InitializeStatus();

        }

        /// <summary>
        /// Menu Button : Open User Management UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_UserManagementBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cv_ContentUC.Children.Clear();
            createUserUC = new UserEdit();
            Canvas.SetTop(createUserUC, 98);
            Canvas.SetLeft(createUserUC, (cv_ContentUC.ActualWidth - createUserUC.Width) / 2);
            core.UserControlOperations.ContentUserControl(cv_ContentUC, createUserUC);
        }

        #endregion

        //! Interface Implements
        #region [interfaces implements]
        void IUserInfoTransfer.Transfer(User user)
        {
            dataContextMainWindow.UserInfo = user;
            this.DataContext = dataContextMainWindow;
            InitializeStatus();
 
        }

        #endregion
    }

    //! Data Transfer Beetwen XAML and C#
    /// <summary>
    /// data transfer beetwen XAML and C#
    /// </summary>
    public class DataContextBundle : NotifyPropertyChanged
    {
        private User _userInfo;

        public User UserInfo
        {
            get { return _userInfo; }
            set
            {
                _userInfo = value;
                OnPropertyChanged("UserInfo");
            }

        }

    }

}
