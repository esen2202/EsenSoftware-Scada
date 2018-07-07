using Scada.core;
using Scada.model;
using Scada.wpf.Classes;
using Scada.wpf.Classes.User;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using Scada.wpf.Pages.Windows;
using System.Windows.Media.Animation;

namespace Scada.wpf.Pages.UserControls.UserManagement
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        DB db;
        User userRecord;
        IUserInfoTransfer mainWindow;
        NotificationPanelCall notifyCall;
        public Login()
        {
            InitializeComponent();
            lbl_WarningMessage.Visibility = Visibility.Hidden;
        }

        private void  Login_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.Capture(this, CaptureMode.SubTree);
            //Mouse.AddLostMouseCaptureHandler(this, OnMouseCaptureLost); // Inputlara girince Mouse Capture Beklemeye Geçiyor. Bu olayı yakalayıp tekrardan Mouse capture açıyorum
            AddHandler();
            txt_uName.Focus();
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

        private void Btn_signIn(object sender, RoutedEventArgs e)
        {
            userRecord = new User();
            db = new core.DB();

            #region user data transfer to MainWindow

            mainWindow = (IUserInfoTransfer)Application.Current.MainWindow;
            if (txt_uName.Text != "" && txt_uPass.Password != "")
            {
                if (db.SignIn(txt_uName.Text, txt_uPass.Password, out userRecord))
                {
                    mainWindow.Transfer(userRecord);
                    notifyCall = new NotificationPanelCall("Login", "Successfully logged in", StatusColor.Success, 3);
                    RemoveThis();
                }
                else
                {
                    lbl_WarningMessage.Visibility = Visibility.Hidden;

                    CustomAnimations.ShakeAnimation(this);
                    if (MainWindow.dataContextMainWindow.UserInfo.Authorization != 0)
                    {
                        mainWindow.Transfer(new User() { User_ID=0, UserName= "No User" , Authorization=0, SuperUser=false}); // Default Data Transfer
                        notifyCall = new NotificationPanelCall("Invalid User", "Username or password is wrong. (+) Current session closed", StatusColor.Warning, 3);
                    }
                    else
                    {
                        notifyCall = new NotificationPanelCall("Invalid User", "Username or password is wrong", StatusColor.Error, 3);
                    }

                    txt_uPass.Password = "";
                    txt_uPass.Focus();
                }
            }
            else
            {
                lbl_WarningMessage.Content = "fields should not be null";
                lbl_WarningMessage.Visibility = Visibility.Visible;

            }
            #endregion

        }
    }
}
