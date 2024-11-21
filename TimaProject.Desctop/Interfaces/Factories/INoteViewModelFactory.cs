using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.Interfaces.Factories
{
    public interface INoteViewModelFactory
    {
        INoteViewModel Create(NoteDTO note);
    }
}
