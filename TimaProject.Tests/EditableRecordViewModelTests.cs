using Xunit;
using TimaProject.ViewModels;
using TimaProject.Models;
using TimaProject.Repositories;
using TimaProject.ViewModels.Factories;
using MvvmTools.Navigation.Services;
using Moq;
using TimaProject.ViewModels.Validators;

namespace TimaProject.Tests
{
    public class EditableRecordViewModelTests
    {
        private readonly RecordRepository _recordRepository;
        private readonly Mock<INavigationService> _mockNavigationService;

        private readonly RecordValidator _validator;

        private Models.Record _record;

        private readonly EditableRecordViewModel _sut;

        public EditableRecordViewModelTests()
        {
            _validator = new RecordValidator();

            _mockNavigationService = new();
            _recordRepository = new RecordRepository();

            _record = new Models.Record(
                DateTime.Parse("27.10.2023 10:00"), 
                DateOnly.Parse("27.10.2023"), 
                1)
            {
                Title = "RecordTitle",
                Project = new Project("MyProject", Guid.NewGuid()),
                EndTime = DateTime.Parse("27.10.2023 20:45")
            };

            _sut = new EditableRecordViewModel(
                    _record,
                    _recordRepository,
                    _mockNavigationService.Object,
                    _mockNavigationService.Object,
                    () => new TimeFormViewModel(
                        _validator,
                        _mockNavigationService.Object),
                    null,
                    _validator);
        }


        [Fact]
        public void EditableRecord_SetValuesFromRecord()
        {
            Assert.Equal(_record.Title, _sut.Title);
            Assert.Equal(_record.StartTime.ToString(), _sut.StartTime);
            Assert.Equal(_record.EndTime.ToString(), _sut.EndTime);
            Assert.Equal(_record.Date.ToString(), _sut.Date);
            Assert.Equal(_record.Project, _sut.Project);
            Assert.Equal("10:45:00", _sut.Time);
        }



        [Fact]
        public void EditableRecord_UpdateCorrectFields()
        {

            _recordRepository.AddRecord(_record);
            var expectedStartTime = DateTime.Parse("26.10.2023 10:00");
            var expectedEndTime = DateTime.Parse("26.10.2023 17:45");
            var expectedDate = DateOnly.Parse("26.10.2023");
            var expectedTilte = "NewTitle";
            var expectedProject = new Project("MySecondProject", Guid.NewGuid());

            _sut.Title = expectedTilte;
            _sut.StartTime = expectedStartTime.ToString();
            _sut.EndTime = expectedEndTime.ToString();
            _sut.Date = expectedDate.ToString();
            _sut.Project = expectedProject;

            var updatedRecord = _recordRepository.GetRecords(new FilterListingArgs()).First();

            Assert.Equal(expectedTilte, updatedRecord.Title);
            Assert.Equal(expectedStartTime, updatedRecord.StartTime);
            Assert.Equal(expectedEndTime, updatedRecord.EndTime);
            Assert.Equal(expectedDate, updatedRecord.Date);
            Assert.Equal(expectedProject, updatedRecord.Project);
        }

        [Fact]
        public void EditableRecord_IgnoreUpdateIncorrectFields()
        {
            _recordRepository.AddRecord(_record);

            _sut.StartTime = "wrong";
            _sut.EndTime = "wrong";
            _sut.Date = "wrong";

            var updatedRecord = _recordRepository.GetRecords(new FilterListingArgs()).First();

            Assert.Equal(_record.StartTime, updatedRecord.StartTime);
            Assert.Equal(_record.EndTime, updatedRecord.EndTime);
            Assert.Equal(_record.Date, updatedRecord.Date);

        }

        [Fact]
        public void DeleteRecord_DeleteRecordFromRepository()
        {
            _recordRepository.AddRecord(_record);

            Assert.True(_recordRepository.Contains(_record));

            _sut.DeleteRecord();

            Assert.False(_recordRepository.Contains(_record));
        }

    }
}
