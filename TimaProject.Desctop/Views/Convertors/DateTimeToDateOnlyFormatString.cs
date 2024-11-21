using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TimaProject.Desctop.Views.Convertors
{
    class DateTimeToDateOnlyFormatString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DateTime.TryParse(value.ToString(), out var date))
            {
                return date;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return string.Empty;
            }
            if (value is DateTime dateTime)
            {
                return DateOnly.FromDateTime(dateTime).ToString();
            }
            throw new ArgumentException();
        }
    }
}
