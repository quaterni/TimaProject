using System;
using System.Windows.Input;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface INoteViewModel
    {
        string Text { get; }
        DateTime Created { get; }
        DateTime LastEdit { get; }
        Guid Id { get; }
        Guid RecordId { get; }
        ICommand DeleteNoteCommand { get; }
    }
}