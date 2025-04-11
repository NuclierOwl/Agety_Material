using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Agentiky.Hardik.Dop
{
    public class ObjectIsNotNullConverter : IValueConverter
    {
        public static readonly ObjectIsNotNullConverter Default = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DiscountToColorConverter : IValueConverter
    {
        public static readonly DiscountToColorConverter Default = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int discount && discount >= 25)
            {
                return Brushes.Green;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}