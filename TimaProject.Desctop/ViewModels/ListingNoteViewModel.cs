using System;
using System.Collections.ObjectModel;
using System.Linq;
using TimaProject.Desctop.Interfaces.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.Factories;

namespace TimaProject.Desctop.ViewModels
{
    public class ListingNoteViewModel : ObservableObject, IListingNoteViewModel
    {
        private readonly Guid _recordId;

        private readonly INoteService _noteService;

        private readonly INoteViewModelFactory _noteViewModelFactory;

        private Lazy<ObservableCollection<INoteViewModel>> _lazyNotes;

        public ObservableCollection<INoteViewModel> Notes
        {
            get
            {
                return _lazyNotes.Value;
            }

        }

        public ListingNoteViewModel(
            Guid recordId, 
            INoteService noteService, 
            INoteViewModelFactory noteViewModelFactory
            )
        {
            _recordId = recordId;
            _noteService = noteService;
            _noteViewModelFactory = noteViewModelFactory;

            _lazyNotes = new Lazy<ObservableCollection<INoteViewModel>>(GetNotes);
        }

        private ObservableCollection<INoteViewModel> GetNotes()
        {
            return new ObservableCollection<INoteViewModel>(
                _noteService.GetNotes(_recordId)
                .OrderBy(n => n.Created)
                .Select(n=> _noteViewModelFactory.Create(n))
                .ToList());
        }
    }
}
