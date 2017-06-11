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
        core.DB db;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (db = new core.DB())
            {
                DG_Users.ItemsSource = db.GetUser();
            }
        }

        private void btn_signIn(object sender, RoutedEventArgs e)
        {
            db = new core.DB();
            if (db.SignIn(txt_uName.Text, txt_uPass.Text))
            {
                MessageBox.Show("Giriş Başarılı");
            }
            else
            {
                MessageBox.Show("Hatalı Giriş");
            }
        }

        private void btn_addUser_Click(object sender, RoutedEventArgs e)
        {
            string message = "";
            model.User user = new model.User
            {
                UserName = txt_uName_new.Text,
                UserPassword = txt_uPass_new.Text,
                SuperUser = false,
                Enable = true,
                Authorization = 1,
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
                        message = "Successfull";
                        break;
                    case 0:
                        message = "Error";
                        break;
                    case -1:
                        message = "Same User already exist";
                        break;

                    default:
                        break;
                }
            }
            MessageBox.Show(message);
        }

        private void btn_getUser_Click(object sender, RoutedEventArgs e)
        {
            Grid_Loaded(sender, e);
        }

        private void DG_Users_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            model.User user = (model.User)DG_Users.SelectedItem;

            txt_uName_new.Text = user.UserName;
            txt_uPass_new.Text = user.UserPassword;
            txt_name_new.Text = user.Name;
            txt_secondName_new.Text = user.SecondName;
            txt_surname_new.Text = user.Surname;
            txt_title_new.Text = user.Title;
            txt_position_new.Text = user.Position;
            txt_eMail_new.Text = user.Email;
            txt_cardID_new.Text = user.CardID;
            

        }
    }
}
