using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.ViewModels
{
    class TimerViewModel :ViewModelBase
    {
        private ulong? _timaNoteId;

        private INoteRepository _noteRepository;

        private string _currentTitle;

        public string CurrentTitle
        {
            get
            {
                return _currentTitle;
            }
            set
            {
                SetValue(ref _currentTitle, value, nameof(CurrentTitle));
            }
        }

        private DateTimeOffset _startingTime;

        public DateTimeOffset StartingTime 
        {
            get 
            {
                return _startingTime;
            }
            set
            {
                SetValue(ref _startingTime, value);
            }
        }

        private Project? _project;

        public Project? Project
        {
            get
            {
                return _project;
            }
            set
            {
                SetValue(ref _project, value);
            }
        }

        public bool IsRunning => _timaNoteId != null;

        public TimerViewModel(INoteRepository noteRepository)
        {
             _currentTitle = string.Empty;
            _startingTime = DateTimeOffset.MinValue;
            _noteRepository = noteRepository;
        }

        public void OnStartingTime()
        {
            ulong id = (ulong)_noteRepository.GetNewId();

            var newNote = new TimaNote(StartingTime, id) 
            { 
                Title = CurrentTitle,
                Project = Project
            };
            _timaNoteId = id;
            _noteRepository.AddNote(newNote);
        }

        public void OnEndingTime()
        {
            if(_timaNoteId is not null)
            {
                var newNote = new TimaNote(StartingTime, _timaNoteId.Value)
                {
                    Title = CurrentTitle,
                    StoppingTime = DateTimeOffset.UtcNow,
                    Project = Project,
                    Date = DateOnly.FromDateTime(DateTime.UtcNow)
                };
                _noteRepository.UpdateNote(newNote);
                SetDafultValues();

            }
        }

        private void SetDafultValues()
        {
            _timaNoteId = null;
            CurrentTitle = string.Empty;
            StartingTime = DateTimeOffset.MinValue;
            Project = null;
        }
    }
}
