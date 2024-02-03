using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.Stores
{
    public class TodayDateStore : IDateStore
    {
        public DateOnly Date => DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
