using Scada.wpf.Classes;
using System.Windows;
using System.Windows.Controls;

namespace Scada.wpf.Pages.UserControls.UserManagement
{
    /// <summary>
    /// Interaction logic for UserEdit.xaml
    /// </summary>
    public partial class UserEdit : UserControl
    {
        public UserEdit()
        {
            InitializeComponent();
        }

        private void OnClickToBuy(object sender, RoutedEventArgs e)
        {
            CheckoutTabControl.SelectedIndex = 3;
        }

        private void UC_EditUser_Loaded(object sender, RoutedEventArgs e)
        {
            core.UserControlOperations.ContentUserControl(gd_CreateNewUser, new CreateUser());
            core.UserControlOperations.ContentUserControl(gd_EditMyProfile, new EditMyProfile());
            core.UserControlOperations.ContentUserControl(gd_EditOtherUser, new EditUsers());
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

        private void Img_CloseBtn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RemoveThis();
        }
    }
}
