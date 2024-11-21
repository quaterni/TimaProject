using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.ViewModels;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace TimaProject.Desctop.Interfaces.Factories
{
    internal interface ITimeFormViewModelFactory
    {
        ITimeFormViewModel Create(TimeDTO timeDTO);
    }
}
