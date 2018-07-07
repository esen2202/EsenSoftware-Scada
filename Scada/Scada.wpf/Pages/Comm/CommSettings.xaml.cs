
using Scada.core;
using Scada.wpf.Pages.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



namespace Scada.wpf.Pages.Comm
{
    /// <summary>
    /// Interaction logic for CommSettings.xaml
    /// </summary>
    public partial class CommSettings : UserControl
    {
        //! Definations
        #region [Definations]

        //- NotificationPanel newNotify;
        NotificationPanelCall notifyCall;

        #endregion

        //! Constructure
        #region [Constructure]
        public CommSettings()
        {
            InitializeComponent();
        }
        #endregion

        //! UserControl 
        #region [UserControl]
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //- Call UserControls to Tabs
            UserControlOperations.ContentUserControl(tab_Settings, new Profinet.Settings());
            UserControlOperations.ContentUserControl(tab_Test, new Profinet.Test());
        }
        #endregion

        private void NotificationShow(string Title, string Message, core.StatusColor Status, int Second)
        {
            notifyCall = new NotificationPanelCall(Title, Message, Status, Second);
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

        private void Img_CloseBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RemoveThis();
        }

        private void OnMouseCaptureLost(object sender, MouseEventArgs e)
        {

        }

        private void Tc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
