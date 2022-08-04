using System;
using System.Windows.Data;

namespace FlipIt.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is DateTime dt)
            {
                if (dt.Year < 0622)//3/22/0622 
                    dt = new DateTime(0622, 3, 22, dt.Hour, dt.Minute, dt.Second);
                return dt.ToString(parameter.ToString(), culture);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}