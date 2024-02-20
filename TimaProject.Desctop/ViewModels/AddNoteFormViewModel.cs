using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.ViewModels
{
    public class AddNoteFormViewModel : ObservableValidator, IAddNoteFormViewModel
    {
        private readonly INoteService _noteService;

        private readonly Guid _recordId;

        private string _text;

        [Required]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                SetProperty(ref _text, value, true);
            }
        }

        public ICommand AddNoteCommand { get; }

        private bool _canAdd;

        public bool CanAdd
        {
            get
            {
                return _canAdd;
            }
            set
            {
                SetProperty(ref _canAdd, value);
            }
        }

        public AddNoteFormViewModel(Guid recordId, INoteService noteService)
        {
            _recordId = recordId;
            _noteService = noteService;

            Text = string.Empty;

            AddNoteCommand = new RelayCommand(AddNote);

            ErrorsChanged += OnErrorsChanged;            
        }

        private void AddNote()
        {
            _noteService.Add(
                new NoteDTO(
                    Text, 
                    Guid.NewGuid(), 
                    _recordId,
                    DateTime.Now,
                    DateTime.Now));
        }

        private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            CanAdd = !HasErrors;
        }        
    }
}
