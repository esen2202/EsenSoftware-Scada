using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Scada.wpf.Pages.Windows
{
    public class NotificationPanelCall
    {
        private  static  NotificationPanel newNotify;
        public NotificationPanelCall(string title, string message, core.StatusColor color, int second)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {

               if(newNotify != null)

                {
                    newNotify.Close();
                    newNotify = new NotificationPanel(title, message, color, second);
                }
                else
                {
                    
                    newNotify = new NotificationPanel(title, message, color, second);
                }

                newNotify.Show();

            });
         
        
        }

    }
}
