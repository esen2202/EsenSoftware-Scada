using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Scada.wpf.Classes
{
    public class CustomAnimations
    {
        public void AnimThickness(DependencyObject target, DependencyProperty property, Thickness from, Thickness to, TimeSpan beginTime, TimeSpan duration, IEasingFunction e)
        {
            ThicknessAnimation animation = new ThicknessAnimation();
            animation.To = to;   // final value

            animation.From = from;


            animation.BeginTime = beginTime;
            animation.Duration = duration;


            animation.EasingFunction = e;

            //start animating
            Storyboard.SetTarget(animation, target);  // what object will be animated?
            Storyboard.SetTargetProperty(animation, new PropertyPath(property)); // what property will be animated
            Storyboard sb = new Storyboard();
            sb.Children.Add(animation);
            sb.Begin();
        }

        public static void ShakeAnimation(UserControl me)
        {
            ThicknessAnimation bounceAnimation = new ThicknessAnimation();
            BounceEase BounceOrientation = new BounceEase();
            BounceOrientation.Bounces = 2;
            BounceOrientation.Bounciness = .3;
            bounceAnimation.To = new Thickness(0, 0, 0, 0);
            bounceAnimation.From = new Thickness(15, 0, 0, 0);
            bounceAnimation.EasingFunction = BounceOrientation;
            me.BeginAnimation(FrameworkElement.MarginProperty, bounceAnimation);
        }

    }
}
