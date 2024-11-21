using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;

namespace TimaProject.Desctop.Interfaces.Services
{
    public interface ITimeService
    {
        TimeServiceResult Solve(string propertyName, TimeDTO timeDTO);
    }
}
