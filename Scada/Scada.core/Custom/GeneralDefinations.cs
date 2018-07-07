using System;
using System.Linq;
using System.Reflection;

namespace Scada.core
{

    public static class GeneralDefinations
    {

        /// <summary>
        /// "x.y" string splitter byte array
        /// </summary>
        /// <param name="AddrStr"></param>
        /// <returns></returns>
        public static byte[] SplitStringToDBX(string AddrStr)
        { 
            var addr = AddrStr.Split('.');
            byte[] result = new byte[2];
            try
            {
                result[0] = (byte)TypeControls.CheckStrToInt(addr[0], 0);
                result[1] = (byte)TypeControls.CheckStrToInt(addr[1], 0);
            }
            catch (Exception)
            {

                result[0] = 0;
                result[1] = 0;
            }

            return result;
        }
    }

    public class TypeControls
    {

        /// <summary>
        /// Str to Int  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int CheckStrToInt(string value, int defaultValue = 0)
        {
            int result;

            if (int.TryParse(value, out int n))
            {
                result = n;
            }
            else
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Str to Dobule  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double CheckStrToDouble(string value, double defaultValue = 0.0)
        {
            double result;
            //double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            if (double.TryParse(value.Replace('.', ','), out double n))
            {
                result = n;
            }
            else
            {
                result = defaultValue;
            }
            return result;
        }


        /// <summary>
        /// Ondalık kısım 0 ise ondalık kısmın ,0 olarak gösterilmesi sağlanır
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DoubleToString(double value)
        {
            string result = "0,0";
            result = value.ToString();
            double a = value % 1;
            if(a == 0)
            {
                result = result + ",0";
            }

            return result;
        }
    }

    #region Enums

    /// <summary>
    /// Status Color Enum
    /// </summary>
    public enum StatusColor
    {
        Success,
        Warning,
        Error
    }

    #endregion

}
