using Scada.core;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Scada.wpf.Pages.Windows
{
    /// <summary>
    /// Interaction logic for NotificationPanel.xaml
    /// </summary>
    public partial class NotificationPanel : Window
    {
        //- public static int notifyCounter;

        public NotificationPanel(string title, string message, StatusColor color, int second)
        {
            //- notifyCounter++;

            #region Initialize
            InitializeComponent();

            NotificationComponents.Title = title;
            NotificationComponents.Message = message;
            NotificationComponents.StatusColor = color;
            NotificationComponents.TimerInterval = second;
            #endregion

            int openNotificationCount = Application.Current.Windows.OfType<NotificationPanel>().Count();

            #region NotificationPanel Location

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Top = desktopWorkingArea.Bottom - this.Height - 25;
            sb_SlideWindow.To = desktopWorkingArea.Right - this.Width; ;

            #endregion

            #region Information Parameters

            switch (NotificationComponents.StatusColor)
            {
                case StatusColor.Success:
                    gd_Content.Background = new SolidColorBrush(Color.FromRgb(92, 184, 92)); // Green
                    break;
                case StatusColor.Warning:
                    gd_Content.Background = new SolidColorBrush(Color.FromRgb(235, 163, 61)); // Yellow
                    break;
                case StatusColor.Error:
                    gd_Content.Background = new SolidColorBrush(Color.FromRgb(217, 83, 79)); // Red
                    break;
                default:
                    gd_Content.Background = new SolidColorBrush(Color.FromRgb(29, 29, 29)); // Darken
                    break;
            }

            lbl_title.Content = NotificationComponents.Title;
            tb_message.Text = NotificationComponents.Message;

            #endregion

            #region Timer
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(NotificationComponents.TimerInterval)
            };

            timer.Tick += delegate (object sender, EventArgs e)
            {
                ((DispatcherTimer)timer).Stop();
                if (this.ShowActivated) this.Close();
            };

            timer.Start();

            #endregion

            #region Animation
            //Storyboard sb = new Storyboard();
            //DoubleAnimation da = new DoubleAnimation(-100, 100, new Duration(new TimeSpan(0, 0, 5)));
            //Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Right)"));
            //sb.Children.Add(da);
            //NotificationWindow.BeginStoryboard(sb);
            #endregion
        }

        private void WidenObject(int newWidth, TimeSpan duration)
        {
            DoubleAnimation animation = new DoubleAnimation(newWidth, duration);
            NotificationWindow.BeginAnimation(Rectangle.WidthProperty, animation);
        }

        private void gd_MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void NotificationWindow_Closed(object sender, EventArgs e)
        {
            //-  notifyCounter--;
        }
    }

    /// <summary>
    /// Notification Context Components
    /// </summary>
    public static class NotificationComponents
    {
        public static StatusColor StatusColor { get; set; }
        public static string Message { get; set; }
        public static string Title { get; set; }
        public static Boolean ShowCloseBtn { get; set; }
        public static int TimerInterval { get; set; }
        public static bool AutoClose { get; set; }

    }


}
