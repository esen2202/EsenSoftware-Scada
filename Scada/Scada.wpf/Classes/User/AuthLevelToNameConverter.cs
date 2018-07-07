using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Scada.wpf.Classes.User
{
    public class AuthLevelToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return AuthorizatonConverter.ConvertAuthorizationString((short)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return AuthorizatonConverter.ConvertAuthorizationLevel((string)value);
        }
    }
}
