using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.Interfaces.Services
{
    public interface IDateReportService
    {
        DateOnly CurrentDate();
        TimeSpan GetTimeAmountPerDate(DateOnly date);
    }
}
