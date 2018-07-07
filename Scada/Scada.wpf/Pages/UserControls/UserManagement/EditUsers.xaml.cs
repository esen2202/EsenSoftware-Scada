using Scada.core;
using Scada.model;
using Scada.wpf.Classes;
using Scada.wpf.Classes.User;
using Scada.wpf.Pages.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scada.wpf.Pages.UserControls.UserManagement
{
    /// <summary>
    /// Interaction logic for EditUsers.xaml
    /// </summary>
    public partial class EditUsers : UserControl
    {
        DB db;
        //- NotificationPanel newNotify;
        NotificationPanelCall notifyCall;
        User CurrentUser;
        User SelectedUser;

        private bool CheckEmpty()
        {
            return (txt_uName_new.Text == "" || txt_CurrentUPass_new.Password == "" || cb_auth_new.SelectedIndex == -1);
        }

        private bool PassDontMatch()
        {
            return txt_uPass_new.Password != "" && (txt_uPass_new.Password != txt_RuPass_new.Password);
        }

        private void UnloadAllFields()
        {
            DataContext = null;
            txt_CurrentUPass_new.Password = "";
            txt_uPass_new.Password = "";
            txt_RuPass_new.Password = "";
        }

        public void LoadUsers()
        {
            #region Get Users
            short AuthControlLevel = (CurrentUser.Authorization < (short)3) ? (short)CurrentUser.Authorization : (short)4;
            cb_users.ItemsSource = null;
            cb_users.Items.Clear();
            cb_users.ItemsSource = db.GetEditableUsers(AuthControlLevel, CurrentUser.User_ID);
            #endregion
        }

        private void LoadAuthorizations()
        {
            #region Load Authorization 
            var AuthLevel = CurrentUser.Authorization;
            if (AuthLevel != 0)
            {
                Dictionary<string, short> AuthList = new Dictionary<string, short>();

                if (AuthLevel > 1)
                    AuthList.Add("Operator", 1);
                if (AuthLevel > 2)
                {
                    AuthList.Add("Engineer", 2);
                    AuthList.Add("Administrator", 3);
                }
                cb_auth_new.ItemsSource = null;
                cb_auth_new.Items.Clear();
                cb_auth_new.ItemsSource = AuthList;
            }
            #endregion
        }

        public EditUsers()
        {
            InitializeComponent();
            DataContext = null;
        }

        private void UC_EditUsers_Loaded(object sender, RoutedEventArgs e)
        {
            db = new DB();
            CurrentUser = MainWindow.dataContextMainWindow.UserInfo;
            wp_editPanel.IsEnabled = false;
            cb_users.IsEnabled = true;

            UnloadAllFields();
 
            LoadUsers();

            LoadAuthorizations();
        }

        private void Btn_SaveUser_Click(object sender, RoutedEventArgs e)
        {
            lbl_WarningMessage.Content = "fields should not be empty";
            lbl_WarningMessage.Visibility = CheckEmpty() ? Visibility.Visible : Visibility.Hidden;

            if (!CheckEmpty())
            {
                var currentPass = core.CryptorEngine.Encrypt(txt_CurrentUPass_new.Password, true);
                if (CurrentUser.UserPassword != currentPass)
                {
                    lbl_WarningMessage.Content = "your password is faulty";
                    lbl_WarningMessage.Visibility = Visibility.Visible;
                    txt_CurrentUPass_new.Password = "";
                    txt_CurrentUPass_new.Focus();
                }
                else
                {
                    if (PassDontMatch())
                    {
                        lbl_WarningMessage.Content = "passwords do not match";
                        lbl_WarningMessage.Visibility = txt_uPass_new.Password != txt_RuPass_new.Password ? Visibility.Visible : Visibility.Hidden;
                        CustomAnimations.ShakeAnimation(this);
                        txt_RuPass_new.Password = "";
                        txt_RuPass_new.Focus();
                    }
                    else
                    {
                        User user = new User
                        {
                            User_ID = SelectedUser.User_ID,
                            UserName = txt_uName_new.Text,
                            UserPassword = txt_uPass_new.Password,
                            SuperUser = false,
                            Enable = true,
                            Authorization = (short)cb_auth_new.SelectedValue,
                            Email = txt_eMail_new.Text,
                            Name = txt_name_new.Text,
                            SecondName = txt_secondName_new.Text,
                            Surname = txt_surname_new.Text,
                            Title = txt_title_new.Text,
                            Position = txt_position_new.Text,
                            PhotoAddress = "",
                            CardID = txt_cardID_new.Text,
                            DateTime = DateTime.Now
                        };

                        using (db = new core.DB())
                        {
                            var result = (int)db.UpdateUser(user);
                            switch (result)
                            {
                                case 1:
                                    notifyCall = new NotificationPanelCall("Update User", "Successfully updated", StatusColor.Success, 3);
                                    RemoveThis();
                                    txt_CurrentUPass_new.Password = "";
                                    Btn_CancelUpdate_Click(sender,e);
                                    LoadUsers();
                                    break;
                                case 0:
                                    notifyCall = new NotificationPanelCall("Error", "Something went wrong", StatusColor.Error, 3);
                                    CustomAnimations.ShakeAnimation(this);
                                    break;
                                case -1:
                                    notifyCall = new NotificationPanelCall("Error", "User is not found", StatusColor.Error, 3);
                                    CustomAnimations.ShakeAnimation(this);
                                    txt_uName_new.Text = "";
                                    txt_uName_new.Focus();
                                    break;
                                case -2:
                                    notifyCall = new NotificationPanelCall("Error", "Same User already exist", StatusColor.Error, 3);
                                    CustomAnimations.ShakeAnimation(this);
                                    txt_uName_new.Text = "";
                                    txt_uName_new.Focus();
                                    break;
                                default:
                                    CustomAnimations.ShakeAnimation(this);
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                CustomAnimations.ShakeAnimation(this);
            }
        }

        private void Btn_DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var resultMsg = MessageBox.Show("Selected user will delete.\n\nAre you sure?", "Delete User", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultMsg == MessageBoxResult.Yes)
            {
                var result = db.DeleteUser((long)cb_users.SelectedValue);
                switch (result)
                {
                    case 1:
                        notifyCall = new NotificationPanelCall("Delete User", "Successfully deleted", StatusColor.Success, 3);
                        UnloadAllFields();
                        cb_users.SelectedIndex = -1;
                        LoadUsers();
                        cb_users.IsEnabled = true;
                        wp_editPanel.IsEnabled = false;
                        break;
                    case 0:
                        notifyCall = new NotificationPanelCall("Error", "User not found", StatusColor.Error, 3);
                        CustomAnimations.ShakeAnimation(this);
                        break;
                    case -1:
                        notifyCall = new NotificationPanelCall("Error", "Something went wrong", StatusColor.Error, 3);
                        CustomAnimations.ShakeAnimation(this);
                        break;
                    default:
                        break;
                }

            }

        }

        private void OnMouseCaptureLost(object sender, MouseEventArgs e)
        {

        }

        private void Bb_users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_users.SelectedValue != null)
            {
                wp_editPanel.IsEnabled = true;
                SelectedUser = db.GetUser((long)cb_users.SelectedValue, CurrentUser.User_ID);
                DataContext = SelectedUser;
                cb_users.IsEnabled = false;
            }
        }

        private void Btn_CancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            UnloadAllFields();
            wp_editPanel.IsEnabled = false;
            SelectedUser = db.GetUser((long)cb_users.SelectedValue, CurrentUser.User_ID);
            DataContext = SelectedUser;
            cb_users.IsEnabled = true;
            cb_users.SelectedIndex = -1;
        }

        private void RemoveThis()
        {
            // find instance of parent element 
            //Canvas parentPanel = UserControlOperations.FindParent<Canvas>(this);
            // remove this from parent element
            //parentPanel.Children.Remove(this);
        }
    }
}
