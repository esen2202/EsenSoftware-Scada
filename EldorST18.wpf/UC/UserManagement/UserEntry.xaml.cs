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
    /// Interaction logic for UserEntry.xaml
    /// </summary>
    public partial class UserEntry : UserControl
    {
        core.DB db;
        public UserEntry()
        {
            InitializeComponent();
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
    }



}
