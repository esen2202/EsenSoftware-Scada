using EldorST18.wpf.UC.Report;
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

namespace EldorST18.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Threading.DispatcherTimer saat = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            saat.Interval = TimeSpan.FromSeconds(1);
            saat.IsEnabled = true;
            saat.Tick += timer_say;
        }

        private void timer_say(object sender, EventArgs e)
        {
            DateTime simdi = DateTime.Now;
            lb_time.Content = simdi.ToLongTimeString();
            lb_date.Content = simdi.ToLongDateString();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bd_titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (e.ClickCount == 2)
            {
                btn_max_Click(sender, e);
            }
        }

        private void btn_max_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {

            }
        }

        private void btn_addUserShow_Click(object sender, RoutedEventArgs e)
        {
            ContentUserControl(wp_content, new UserAdd());
        }

        private void btn_entryUserShow_Click(object sender, RoutedEventArgs e)
        {
            ContentUserControl(wp_content, new UserEntry());
        }

        private void btn_editUserShow_Click(object sender, RoutedEventArgs e)
        {
            ContentUserControl(wp_content, new UserEdit());
        }

        /// <summary>
        /// Call User Control in Grid
        /// </summary>
        /// <param name="panel">Grid</param>
        /// <param name="uc">UserControl</param>
        private void ContentUserControl(WrapPanel panel, UserControl uc)
        {
            if (panel.Children.Count > 0)
            {
                panel.Children.Clear();
            }
            panel.Children.Add(uc);
        }

        private void img_extendMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridLength gl80 = new GridLength(80, GridUnitType.Pixel);
            GridLength gl250 = new GridLength(250, GridUnitType.Pixel);

            if (gc_sideMenu.Width == gl80)
            {
                gc_sideMenu.Width = gl250;
            }
            else
            {
                gc_sideMenu.Width = gl80;
            }
        }

        private void mbtn_reporting_Click(object sender, RoutedEventArgs e)
        {
            ContentUserControl(wp_content, new Reporting());
        }
    }
}
