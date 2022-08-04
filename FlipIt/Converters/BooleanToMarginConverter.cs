using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FlipIt.Converters
{
    [ValueConversion(typeof(bool), typeof(Thickness))]
    public class BooleanToMarginConverter : IValueConverter
    {
        public Thickness TrueValue { get; set; }
        public Thickness FalseValue { get; set; }

        public BooleanToMarginConverter()
        {
            // set defaults
            TrueValue = new(1);
            FalseValue = new(0);
        }

        public object? Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return null;
            return (bool)value ? TrueValue : FalseValue;
        }

        public object? ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (Equals(value, TrueValue))
                return true;
            if (Equals(value, FalseValue))
                return false;
            return null;
        }
    }
}