using System.Windows.Input;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface IAddNoteFormViewModel
    {
        string Text { get; set; }
        ICommand AddNoteCommand { get; }
        bool CanAdd { get; }
    }
}