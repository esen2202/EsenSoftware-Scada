using Scada.core;
using Scada.model;
using Scada.wpf.Classes;
using Scada.wpf.Classes.User;
using Scada.wpf.Pages.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scada.wpf.Pages.UserControls.UserManagement
{
    /// <summary>
    /// Interaction logic for EditMyProfile.xaml
    /// </summary>
    public partial class EditMyProfile : UserControl
    {
        DB db;
        //- NotificationPanel newNotify;
        NotificationPanelCall notifyCall;
        User CurrentUser;
        public EditMyProfile()
        {
            InitializeComponent();
        }

        private bool CheckEmpty()
        {
            return (txt_uName_new.Text == "" || txt_CurrentUPass_new.Password == "" || cb_auth_new.SelectedIndex == -1);
        }

        private bool PassDontMatch()
        {
            return txt_uPass_new.Password != "" && (txt_uPass_new.Password != txt_RuPass_new.Password);
        }

        private void Btn_SaveMyProfil_Click(object sender, RoutedEventArgs e)
        {
            lbl_WarningMessage.Content = "fields should not be empty";
            lbl_WarningMessage.Visibility = CheckEmpty() ? Visibility.Visible : Visibility.Hidden;

            if (!CheckEmpty())
            {
                var currentPass = core.CryptorEngine.Encrypt(txt_CurrentUPass_new.Password, true);
                if (CurrentUser.UserPassword != currentPass)
                {
                    lbl_WarningMessage.Content = "current password is faulty";
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
                            User_ID = CurrentUser.User_ID,
                            UserName = txt_uName_new.Text,
                            UserPassword = (txt_uPass_new.Password != "") ? txt_uPass_new.Password : CryptorEngine.Decrypt(CurrentUser.UserPassword, true),
                            SuperUser = false,
                            Enable = true,
                            Authorization = CurrentUser.Authorization,
                            Email = txt_eMail_new.Text,
                            Name = txt_name_new.Text,
                            SecondName = txt_secondName_new.Text,
                            Surname = txt_surname_new.Text,
                            Title = txt_title_new.Text,
                            Position = txt_position_new.Text,
                            PhotoAddress = "",
                            CardID = txt_cardID_new.Text,
                            DateTime = CurrentUser.DateTime
                        };

                        using (db = new core.DB())
                        {
                            var result = db.UpdateUser(user);
                            switch (result)
                            {
                                case 1:
                                    notifyCall = new NotificationPanelCall("Update User", "Successfully updated", StatusColor.Success, 3);
                                    RemoveThis();
                                    txt_CurrentUPass_new.Password = "";
                                    // UnloadAllFields();
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
                txt_CurrentUPass_new.Focus();
            }
        }

        private void OnMouseCaptureLost(object sender, MouseEventArgs e)
        {
            // Null
        }

        private void UC_EditMyProfile_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentUser = MainWindow.dataContextMainWindow.UserInfo;

            var AuthLevel = MainWindow.dataContextMainWindow.UserInfo.Authorization;
            Dictionary<string, short> AuthList = new Dictionary<string, short>();
            AuthList.Add(AuthorizatonConverter.ConvertAuthorizationString((short)AuthLevel), (short)AuthLevel);
            cb_auth_new.ItemsSource = null;
            cb_auth_new.Items.Clear();
            cb_auth_new.ItemsSource = AuthList;
            cb_auth_new.SelectedIndex = 0;
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
