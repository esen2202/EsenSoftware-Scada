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

        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

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
            ContentUserControl(gd_content, new UserAdd());
        }

        private void btn_entryUserShow_Click(object sender, RoutedEventArgs e)
        {
            ContentUserControl(gd_content, new UserEntry());
        }

        private void btn_editUserShow_Click(object sender, RoutedEventArgs e)
        {
            ContentUserControl(gd_content, new UserEdit());
        }

        /// <summary>
        /// Call User Control in Grid
        /// </summary>
        /// <param name="grid">Grid</param>
        /// <param name="uc">UserControl</param>
        private void ContentUserControl(Grid grid, UserControl uc)
        {
            if (grid.Children.Count > 0)
            {
                grid.Children.Clear();
            }
            grid.Children.Add(uc);
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
    }
}
