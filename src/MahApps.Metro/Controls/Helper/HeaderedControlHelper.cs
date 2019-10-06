using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
    using System.ComponentModel;

    public static class HeaderedControlHelper
    {
        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.RegisterAttached("HeaderForeground", typeof(Brush), typeof(HeaderedControlHelper), new UIPropertyMetadata(Brushes.White));

        [Category(AppName.MahApps)]
        [AttachedPropertyBrowsableForType(typeof(GroupBox))]
        [AttachedPropertyBrowsableForType(typeof(Expander))]
        public static Brush GetHeaderForeground(UIElement element)
        {
            return (Brush)element.GetValue(HeaderForegroundProperty);
        }

        public static void SetHeaderForeground(UIElement element, Brush value)
        {
            element.SetValue(HeaderForegroundProperty, value);
        }


        public static readonly DependencyProperty UseCornerRadiusAtHeaderProperty =
            DependencyProperty.RegisterAttached("UseCornerRadiusAtHeader", typeof(bool), typeof(HeaderedControlHelper), new UIPropertyMetadata(false));

        [Category(AppName.MahApps)]
        [AttachedPropertyBrowsableForType(typeof(GroupBox))]
        [AttachedPropertyBrowsableForType(typeof(Expander))]
        public static bool GetUseCornerRadiusAtHeader(UIElement element)
        {
            return (bool)element.GetValue(UseCornerRadiusAtHeaderProperty);
        }

        public static void SetUseCornerRadiusAtHeader(UIElement element, bool value)
        {
            element.SetValue(UseCornerRadiusAtHeaderProperty, value);
        }
    }
}
