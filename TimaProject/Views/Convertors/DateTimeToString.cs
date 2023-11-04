using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TimaProject.Views.Convertors
{
    class DateTimeToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Equals(string.Empty))
            {
                return DateTime.MinValue;
            }

            if(DateTime.TryParse(value.ToString(), out var date))
            {
                return date;
            }
            throw new NotImplementedException();
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTime date)
            {
                return date.ToString();
            }
            return string.Empty;
        }
    }
}
