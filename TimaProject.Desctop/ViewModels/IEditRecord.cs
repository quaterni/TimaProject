using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimaProject.Desctop.ViewModels
{
    internal interface IEditRecord
    {
        ICommand OpenTimeFormCommand { get; }
        ICommand OpenProjectFormCommand { get; }
    }
}
