using Scada.core;
using Scada.model;
using Scada.wpf.Classes;
using Scada.wpf.Pages.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Scada.wpf.Pages.UserControls.UserManagement
{
    /// <summary>
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : UserControl
    {
        DB db;
        //- NotificationPanel newNotify;
        NotificationPanelCall notifyCall;
        public CreateUser()
        {
            InitializeComponent();
        }

        private void CreateUser_Loaded(object sender, RoutedEventArgs e)
        {
            var AuthLevel = MainWindow.dataContextMainWindow.UserInfo.Authorization;
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
            txt_uName_new.Focus();
        }

        #region UserControl Controls
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            RemoveThis();
        }

        private void RemoveThis()
        {
            // find instance of parent element 
            //Canvas parentPanel = UserControlOperations.FindParent<Canvas>(this);
            // remove this from parent element
            //parentPanel.Children.Remove(this);
        }

        #endregion

        private void UnloadAllFields()
        {
            txt_uName_new.Text = "";
            txt_uPass_new.Password = "";
            txt_RuPass_new.Password = "";
            txt_CurrentUPass_new.Password = "";
            txt_name_new.Text = "";
            txt_secondName_new.Text = "";
            txt_surname_new.Text = "";
            txt_eMail_new.Text = "";
            txt_title_new.Text = "";
            txt_position_new.Text = "";
            txt_cardID_new.Text = "";

        }

        private bool CheckEmpty()
        {
            return
            (txt_uName_new.Text == "" ||
            txt_uPass_new.Password == "" ||
            txt_RuPass_new.Password == "" ||
            cb_auth_new.SelectedIndex == -1);
        }

        private void OnMouseCaptureLost(object sender, MouseEventArgs e)
        {

        }

        private void Btn_CreateUser_Click(object sender, RoutedEventArgs e)
        {
            lbl_WarningMessage.Content = "fields should not be empty";
            lbl_WarningMessage.Visibility = CheckEmpty() ? Visibility.Visible : Visibility.Hidden;

            if (!CheckEmpty())
            {
                if (txt_uPass_new.Password != txt_RuPass_new.Password)
                {
                    lbl_WarningMessage.Content = "passwords do not match";
                    lbl_WarningMessage.Visibility = txt_uPass_new.Password != txt_RuPass_new.Password ? Visibility.Visible : Visibility.Hidden;
                    CustomAnimations.ShakeAnimation(this);
                }
                else
                {
                    User user = new User
                    {
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
                        var result = db.AddUser(user);
                        switch (result)
                        {
                            case 1:
                                notifyCall = new NotificationPanelCall("Create User", "Successfully created", StatusColor.Success, 3);
                                RemoveThis();
                                UnloadAllFields();
                                break;
                            case 0:
                                notifyCall = new NotificationPanelCall("Error", "Something went wrong", StatusColor.Error, 3);
                                CustomAnimations.ShakeAnimation(this);
                                break;
                            case -1:
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
            else
            {
                CustomAnimations.ShakeAnimation(this);

            }
        }


    }


}
