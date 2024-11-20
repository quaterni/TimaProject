using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using CommunityToolkit.Mvvm.Input;
using TimaProject.Desctop.Interfaces.Services;

namespace TimaProject.Desctop.ViewModels
{
    internal class RecordViewModel : RecordViewModelBase, IRecordViewModel
    {
        private readonly IRecordService _recordService;

        private bool _isNoteExpanded;

        public bool IsNoteExpanded
        {
            get { return _isNoteExpanded; }
            set { SetProperty(ref _isNoteExpanded, value); }
        }

        private Lazy<IAddNoteFormViewModel> _lazyAddNoteViewModel;

        public IAddNoteFormViewModel AddNoteViewModel => _lazyAddNoteViewModel.Value;

        private Lazy<IListingNoteViewModel> _lazyListingNoteViewModel;

        public IListingNoteViewModel ListingNoteViewModel => _lazyListingNoteViewModel.Value;

        public Guid Id { get; }

        public ICommand DeleteRecordCommand { get; }

        public RecordViewModel(
            RecordDto record,
            ITimeFormViewModelFactory timeFormFactory,
            IProjectFormViewModelFactory projectFormViewModelFactory,
            IAddNoteFormFactory noteFormViewModelFactory,
            IListingNoteViewModelFactory listingNoteViewModelFactory,
            IRecordService recordService): base(timeFormFactory, projectFormViewModelFactory)
        {
            if (record.IsActive)
            {
                throw new Exception("Record must be not active");
            }

            Title = record.Title;
            StartTime = record.StartTime;
            EndTime = record.EndTime!;
            Time = record.Time!;
            Date = record.Date;
            Id = record.RecordId;
            ProjectName = record.ProjectName;
            ProjectId = record.ProjectId;
            _isNoteExpanded = false;

            DeleteRecordCommand = new RelayCommand(OnDeleteRecord);

            _recordService = recordService;

            _lazyAddNoteViewModel = new Lazy<IAddNoteFormViewModel>(
                () => noteFormViewModelFactory.Create(Id));

            _lazyListingNoteViewModel = new Lazy<IListingNoteViewModel>(
                () => listingNoteViewModelFactory.Create(Id));
        }

        private void OnDeleteRecord()
        {
            _recordService.DeleteRecord(Id);
        }


        // TODO: improve it
        private void OnRecordUpdated(object? sender, PropertyChangedEventArgs e)
        {
            string[] properties = [
                nameof(StartTime),
                nameof(Title),
                nameof(ProjectName),
                nameof(ProjectId),
                nameof(Date),
                nameof(EndTime),
                nameof(Time)
            ];
            if(properties.Contains(e.PropertyName))
            {
                UpdateRecord();
            }
        }

        private void UpdateRecord()
        {
            _recordService.UpdateRecord(
                new RecordDto(StartTime, Title, Id)
                {
                    EndTime = EndTime,
                    Date = Date,
                    ProjectId = ProjectId,
                    ProjectName = ProjectName,
                    Time = Time,
                    IsActive = false
                });
        }
    }
}
