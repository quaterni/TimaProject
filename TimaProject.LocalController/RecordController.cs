using TimaProject.DataAccess.Repositories;
using TimaProject.Domain.Models;

namespace TimaProject.LocalController
{
    public record RecordContainer(string StartTime, string Title, Guid RecordId)
    {
        public bool IsActive { get; init; }
        public Guid ProjectId { get; init; } = Guid.Empty;
        public string? EndTime { get; init; }
        public string Date { get; init; }
        public string ProjectName { get; init; }
        public string? Time { get; init; }
    }

    public class RecordController
    {
        private readonly RecordRepository _recordRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly NoteRepository _notesRepository;

        public RecordController()
        {
            _recordRepository = Store.GetRecordRepository();
            _recordRepository.RepositoryChanged += OnRepositoryChanged;
            _projectRepository = Store.GetProjectRepository();
            _notesRepository = Store.GetNoteRepository();
        }

        private void OnRepositoryChanged(object? sender, RepositoryChangedEventArgs<Record> e)
        {
            RecordChanged?.Invoke(this, new RepositoryChangedEventArgs<RecordContainer>(ToFrom(e.Item), e.Operation));
        }

        public event EventHandler<RepositoryChangedEventArgs<RecordContainer>>? RecordChanged;

        public void AddRecord(RecordContainer record)
        {
            _recordRepository.AddItem(FromTo(record));
        }

        public bool DeleteRecord(Guid recordId)
        {
            return _recordRepository.DeleteItem(recordId);
        }

        public IEnumerable<RecordContainer> GetRecords()
        {
            return _recordRepository.GetItems().Select(ToFrom);
        }

        public void UpdateRecord(RecordContainer record)
        {
            _recordRepository.UpdateItem(FromTo(record));
        }

        private Record FromTo(RecordContainer recordContainer)
        {
            var startTime = DateTime.Parse(recordContainer.StartTime);
            DateTime? endTime = recordContainer.IsActive? null : DateTime.Parse(recordContainer.EndTime!);
            var project = _projectRepository.GetProject(recordContainer.ProjectId) ?? Project.Empty;
            var date = DateOnly.Parse(recordContainer.Date);
            return new Record(startTime, date, recordContainer.RecordId)
            {
                EndTime = endTime,
                Project = project,
                Notes = _notesRepository.GetItems(note => note.RecordId == recordContainer.RecordId).ToList(),
                Title = recordContainer.Title
            };
        }

        private RecordContainer ToFrom(Record record)
        {
            return new RecordContainer(record.StartTime.ToString(), record.Title, record.Id)
            {
                EndTime = record.EndTime?.ToString() ?? null,
                IsActive = record.IsActive,
                Date = record.Date.ToString(),
                Time = (record.EndTime - record.StartTime).ToString(),
                ProjectName = record.Project.Name,
                ProjectId = record.Project.Id
            };
        }
    }
}
