using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Stores
{
    public interface IDateStore
    {
        public DateOnly Date { get; }

    }
}
