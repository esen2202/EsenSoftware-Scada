using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EldorST18.wpf.Classes.Converters
{
    public class SideBarVisibilityConverter : IValueConverter
    {
     
        /// <summary>
        /// Side Bar - Menu -  change visibility icon and label 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
       public object  Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (GridLength)value;
            if (width.Value > 80)
            {
                return Visibility.Visible; // big icon and label
            } else
            {
                return Visibility.Collapsed; ; // only small icon 
            }
        }

        public  object  ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SideBarVisibilityReverseConverter : IValueConverter
    {

        /// <summary>
        /// Side Bar - Menu -  change visibility icon and label 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (GridLength)value;
            if (width.Value > 80)
            {
                return Visibility.Collapsed; // big icon and label
            }
            else
            {
                return Visibility.Visible; ; // only small icon 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SideBarBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (GridLength)value;
            if (width.Value > 80)
            {
                return true; // big icon and label
            }
            else
            {
                return false; ; // only small icon 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SideBarRotateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (GridLength)value;
            if (width.Value > 80)
            {
                return 0; // big icon and label
            }
            else
            {
                return -90; ; // only small icon 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SideBarOrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (GridLength)value;
            if (width.Value > 80)
            {
                return Orientation.Vertical; // big icon and label
            }
            else
            {
                return Orientation.Horizontal; // only small icon 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
