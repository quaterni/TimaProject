using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TimaProject.Domain.Models;
using TimaProject.DataAccess.Repositories;

namespace TimaProject.Desctop.Views.Convertors
{
    public class EmptyProjectToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Project project)
            {
                throw new ArgumentException();
            }
            return project.Equals(Project.Empty);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
