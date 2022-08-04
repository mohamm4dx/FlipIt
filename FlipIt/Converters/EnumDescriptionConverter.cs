using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace FlipIt.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumDescriptionConverter : IValueConverter
    {
        private string? GetEnumDescription(Enum enumObj)
        {
            var attribArray = enumObj.GetType().GetField(enumObj.ToString())
                ?.GetCustomAttributes(false);

            if (attribArray?.Length == 0) return enumObj.ToString();

            var attrib = (DescriptionAttribute?)attribArray?[0];
            return attrib?.Description;
        }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetEnumDescription((Enum)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}