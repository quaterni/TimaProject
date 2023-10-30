using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TimaProject.Views.Convertors
{
    class DateTimeOffsetToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Equals(string.Empty))
            {
                return DateTimeOffset.MinValue;
            }

            if(DateTimeOffset.TryParse(value.ToString(), out var date))
            {
                return date;
            }
            throw new NotImplementedException();
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTimeOffset date)
            {
                return date.ToString();
            }
            return null;
        }
    }
}
