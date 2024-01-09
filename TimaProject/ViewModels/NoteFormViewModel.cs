using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimaProject.Commands;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.ViewModels
{
    public class NoteFormViewModel : NotifyDataErrorViewModel
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly Record _source;

        private bool _isCanAdd;

        public bool IsCanAdd
        {
            get 
            {
                return _isCanAdd;
            }
            set
            {
                SetValue(ref _isCanAdd, value);
            }
        }


        private string _text;

        public string Text
        {
            get
            {
                return _text; 
            }
            set
            { 
                SetValue(ref _text, value);
            }
        }


        public ICommand AddNewNoteCommand { get; }

        public NoteFormViewModel(Record source, IRepository<Note> noteRepository)
        {
            _source = source;
            _noteRepository = noteRepository;
            _text = string.Empty;
            AddNewNoteCommand = new CommandCallback(_ => AddNote());
            PropertyChanged += OnCanAddChanged;
        }

        private void OnCanAddChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(HasErrors))
            {
                IsCanAdd = !HasErrors;
            }
        }

        private void AddNote()
        {
            var newNote = new Note(_text, Guid.NewGuid(), _source.Id, DateTime.Now);
            _noteRepository.AddItem(newNote);
        }

        protected override void Validate(string propertyName)
        {
            if(propertyName == nameof(Text) && _text == string.Empty)
            {
                ClearErrors(nameof(Text));
                AddError(nameof(Text), "Text must be not empty.");
            }
        }
    }
}
