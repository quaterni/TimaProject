using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface IDialog : IDisposable
    {
        ICommand CloseCommand { get; }
        event EventHandler<EventArgs> Closed;
    }
}
