using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
    /// <summary>
    /// This class is only used by HeaderedControls (e.g. GroupBox and Expander) 
    /// Converts a Thickness to a negative Margin. It's possible to ignore a side with the IgnoreThickness property.
    /// </summary> 

    [ValueConversion(typeof(Thickness), typeof(Thickness), ParameterType = typeof(ThicknessSideType))]
    public class BorderThicknessToNegativeMarginConverter : IValueConverter
    {
        ThicknessSideType ThicknessSideType { get; set; } = ThicknessSideType.None; 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
            {
                if (parameter is ThicknessSideType thicknessSideType)
                {
                    ThicknessSideType = thicknessSideType;
                }

                switch (ThicknessSideType)
                {
                    case ThicknessSideType.Left:
                        return new Thickness(0, -thickness.Top, -thickness.Right, -thickness.Bottom);
                    case ThicknessSideType.Top:
                        return new Thickness(-thickness.Left, 0, -thickness.Right, -thickness.Bottom);
                    case ThicknessSideType.Right:
                        return new Thickness(-thickness.Left, -thickness.Top, 0, -thickness.Bottom);
                    case ThicknessSideType.Bottom:
                        return new Thickness(-thickness.Left, -thickness.Top, -thickness.Right, 0);
                    default:
                        return new Thickness(-thickness.Left, -thickness.Top, -thickness.Right, -thickness.Bottom);
                }
            }

            return default(Thickness);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // for now no back converting
            return DependencyProperty.UnsetValue;
        }
    }
}
