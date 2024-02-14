using System;
using System.Windows.Input;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface INoteViewModel
    {
        string Text { get; }
        Guid Id { get; }
        ICommand DeleteNoteCommand { get; }
    }
}