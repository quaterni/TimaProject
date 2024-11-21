using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface IRecordViewModel : IRecordViewModelBase
    {
        Guid Id { get; }
        ICommand DeleteRecordCommand { get; }
        IListingNoteViewModel ListingNoteViewModel { get;}
        IAddNoteFormViewModel AddNoteViewModel { get; }
    }
}
