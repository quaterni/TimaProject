using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels.Factories;

namespace TimaProject.ViewModels
{
    public class ListingNoteViewModel : ViewModelBase
    {
        private readonly Func<Note, bool> _noteFilter;
        private readonly IRepository<Note> _noteRepository;
        private readonly EditableNoteViewModelFactory _editableNoteFactory;
        private ObservableCollection<EditableNoteViewModel> _notes;
        

        public ObservableCollection<EditableNoteViewModel> Notes
        {
            get
            { 
                return _notes;
            }
            set 
            {
                SetValue(ref _notes, value);
            }
        }


        public ListingNoteViewModel(
            Func<Note, bool> noteFilter,
            IRepository<Note> noteRepository,
            EditableNoteViewModelFactory editableNoteFactory
            )
        {
            _notes = new ObservableCollection<EditableNoteViewModel>();
            _editableNoteFactory = editableNoteFactory;
            _noteFilter = noteFilter;
            _noteRepository = noteRepository;
            _editableNoteFactory = editableNoteFactory;
            _noteRepository.RepositoryChanged += OnListingChanged;
            OnListingChanged(this, EventArgs.Empty);
        }

        private void OnListingChanged(object? sender, EventArgs e)
        {
            var editableNotes = _noteRepository
                .GetItems(_noteFilter)
                .OrderBy(note => note.Created)
                .Select(note => _editableNoteFactory.Create(note))
                .ToList();
            Notes = new ObservableCollection<EditableNoteViewModel> (editableNotes);
        }
    }
}
