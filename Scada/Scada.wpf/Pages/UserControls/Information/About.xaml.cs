using Scada.wpf.Classes;
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

namespace Scada.wpf.Pages.UserControls.Information
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.Capture(this, CaptureMode.SubTree);
            //Mouse.AddLostMouseCaptureHandler(this, OnMouseCaptureLost); // Inputlara girince Mouse Capture Beklemeye Geçiyor. Bu olayı yakalayıp tekrardan Mouse capture açıyorum
            AddHandler();
        }

        /// <summary>
        /// add mouse click control outside event
        /// </summary>
        private void AddHandler()
        {
            AddHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, new MouseButtonEventHandler(HandleClickOutsideOfControl), true);
        }

        /// <summary>
        /// remove this when mouse click outside of UserControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleClickOutsideOfControl(object sender, MouseEventArgs e)
        {
            RemoveThis();
            // release mouse capturing
            ReleaseMouseCapture();
        }

        /// <summary>
        /// re-capturing when lost Mouse.Capture handled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseCaptureLost(object sender, MouseEventArgs e)
        {
            Mouse.Capture(this, CaptureMode.SubTree);
        }

        /// <summary>
        /// remove UserControl from Parent Element
        /// </summary>
        private void RemoveThis()
        {
            // find instance of parent element 
            Canvas parentPanel = core.UserControlOperations.FindParent<Canvas>(this);
            // remove this from parent element
            parentPanel.Children.Remove(this);
        }
    }

}
