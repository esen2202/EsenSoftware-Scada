using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Scada.core
{
    public class UserControlOperations
    {
        /// <summary>
        /// Get Parent Object Instance of UserContol
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            T parent = VisualTreeHelper.GetParent(child) as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parent);
        }

        /// <summary>
        /// Call User Control in Canvas
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="uc"></param>
        public static void ContentUserControl(Canvas panel, UserControl uc, bool clear = true)
        {
            if (panel.Children.Count > 0 && clear)
            {
                panel.Children.Clear();
            }
            panel.Children.Add(uc);
        }

        /// <summary>
        /// Call User Control in WrapPanel
        /// </summary>
        /// <param name="panel">Grid</param>
        /// <param name="uc">UserControl</param>
        public static void ContentUserControl(WrapPanel panel, UserControl uc, bool clear = true)
        {
            if (panel.Children.Count > 0 && clear)
            {
                panel.Children.Clear();
            }
            panel.Children.Add(uc);
        }

        /// <summary>
        /// Call User Control in StackPanel
        /// </summary>
        /// <param name="panel">Grid</param>
        /// <param name="uc">UserControl</param>
        public static void ContentUserControl(StackPanel panel, UserControl uc, bool clear = true)
        {
            if (panel.Children.Count > 0 && clear)
            {
                panel.Children.Clear();
            }
            panel.Children.Add(uc);
        }

        /// <summary>
        /// Call User Control in DockPanel
        /// </summary>
        /// <param name="panel">Grid</param>
        /// <param name="uc">UserControl</param>
        public static void ContentUserControl(DockPanel panel, UserControl uc, bool clear = true)
        {
            if (panel.Children.Count > 0 && clear)
            {
                panel.Children.Clear();
            }
            panel.Children.Add(uc);
        }

        /// <summary>
        /// Call User Control in Grid
        /// </summary>
        /// <param name="panel">Grid</param>
        /// <param name="uc">UserControl</param>
        public static void ContentUserControl(Grid panel, UserControl uc, bool clear = true)
        {
            if (panel.Children.Count > 0 && clear)
            {
                panel.Children.Clear();
            }
            panel.Children.Add(uc);
        }
    }
}
