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
    /// Interaction logic for UserAdd.xaml
    /// </summary>
    public partial class UserAdd : UserControl
    {
        core.DB db;
        public UserAdd()
        {
            InitializeComponent();
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
    }

}
