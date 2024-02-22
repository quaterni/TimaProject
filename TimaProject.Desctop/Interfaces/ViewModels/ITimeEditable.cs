using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface ITimeEditable
    {
        ICommand OpenTimeFormCommand { get; }
        ITimeFormViewModel? TimeFormViewModel { get; }
        bool IsTimeFormOpened { get; }
    }
}
