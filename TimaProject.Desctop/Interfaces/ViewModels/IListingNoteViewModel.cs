using System.Collections.ObjectModel;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface IListingNoteViewModel
    {
        ObservableCollection<INoteViewModel> Notes {get;}
    }
}