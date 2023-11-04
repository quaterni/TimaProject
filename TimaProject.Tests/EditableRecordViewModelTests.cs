using Xunit;
using TimaProject.ViewModels;
using TimaProject.Models;
using TimaProject.Repositories;

namespace TimaProject.Tests
{
    public class EditableRecordViewModelTests
    {
        private readonly MockRecordRepository _recordRepository;

        public EditableRecordViewModelTests()
        {
            _recordRepository = new MockRecordRepository();
        }

        [Fact]
        public void EditableRecordShould_SetValuesFromRecord()
        {
            var record = new Models.Record(DateTime.Parse("27.10.2023 10:00"), DateOnly.Parse("27.10.2023"), 1)
            {
                Title = "RecordTitle",
                Project = new Project("MyProject", 1),
                EndTime = DateTime.Parse("27.10.2023 20:45")
            };
            _recordRepository.AddRecord(record);
            var sut = new EditableRecordViewModel(
                record,
                _recordRepository,
                new MockNavigationService(),
                () => new TimeFormViewModel(
                        new MockRecordValidator(),
                        new MockNavigationService()),
                new MockRecordValidator());
            Assert.Equal(record.Title, sut.Title);
            Assert.Equal(record.StartTime.ToString(), sut.StartTime);
            Assert.Equal(record.EndTime.ToString(), sut.EndTime);
            Assert.Equal(record.Date.ToString(), sut.Date);
            Assert.Equal(record.Project, sut.Project);
        }

        [Fact]
        public void EditableRecordShould_UpdateCorrectFields()
        {
            var record = new Models.Record(DateTime.Parse("27.10.2023 10:00"), DateOnly.Parse("27.10.2023"), 1)
            {
                Title = "RecordTitle",
                Project = new Project("MyProject", 1),
                EndTime = DateTime.Parse("27.10.2023 20:45")
               
            };

            _recordRepository.AddRecord(record);

            var sut = new EditableRecordViewModel(
                record,
                _recordRepository,
                new MockNavigationService(),
                () => new TimeFormViewModel(
                        new MockRecordValidator(),
                        new MockNavigationService()),
                new MockRecordValidator());

            var expectedStartTime = DateTime.Parse("26.10.2023 10:00");
            var expectedEndTime = DateTime.Parse("26.10.2023 17:45");
            var expectedDate = DateOnly.Parse("26.10.2023");
            var expectedTilte = "NewTitle";
            var expectedProject = new Project("MySecondProject", 2);
            var expectedTime = "07:45:00";

            sut.Title = expectedTilte;
            sut.StartTime = expectedStartTime.ToString();
            sut.EndTime = expectedEndTime.ToString();
            sut.Date = expectedDate.ToString();
            sut.Project = expectedProject;

            var updatedRecord = _recordRepository.Notes[0];

            Assert.Equal(expectedTilte, updatedRecord.Title);
            Assert.Equal(expectedStartTime, updatedRecord.StartTime);
            Assert.Equal(expectedEndTime, updatedRecord.EndTime);
            Assert.Equal(expectedDate, updatedRecord.Date);
            Assert.Equal(expectedProject, updatedRecord.Project);
            Assert.Equal(expectedTime, sut.Time);
        }

        [Fact]
        public void EditableRecordShould_IgnoreUpdateIncorrectFields()
        {
            var record = new Models.Record(DateTime.Parse("27.10.2023 10:00"), DateOnly.Parse("27.10.2023"), 1)
            {
                Title = "RecordTitle",
                Project = new Project("MyProject", 1),
                EndTime = DateTime.Parse("27.10.2023 20:45")
            };

            _recordRepository.AddRecord(record);

            var sut = new EditableRecordViewModel(
                record,
                _recordRepository,
                new MockNavigationService(),
                () => new TimeFormViewModel(
                        new MockRecordValidator(),
                        new MockNavigationService()),
                new MockRecordValidator());

            sut.StartTime = "wrong";
            sut.EndTime = "wrong";
            sut.Date = "wrong";

            var updatedRecord = _recordRepository.Notes[0];

            Assert.Equal(record.StartTime, updatedRecord.StartTime);
            Assert.Equal(record.EndTime, updatedRecord.EndTime);
            Assert.Equal(record.Date, updatedRecord.Date);

        }


    }
}
