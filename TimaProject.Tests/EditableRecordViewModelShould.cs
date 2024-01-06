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
    public class EditableRecordViewModelShould
    {
        private readonly Mock<IRecordRepository> _mockRecordRepository;
        private readonly Mock<INavigationService> _mockNavigationService;

        private readonly TimeValidator _validator;

        private Models.Record _record;

        private readonly EditableRecordViewModel _sut;

        public EditableRecordViewModelShould()
        {
            _mockRecordRepository = new Mock<IRecordRepository>();
            _validator = new TimeValidator();

            _mockNavigationService = new();

            _record = new Models.Record(
                DateTime.Parse("27.10.2023 10:00"),
                DateOnly.Parse("27.10.2023"),
                Guid.NewGuid()
                )
            {
                Title = "RecordTitle",
                Project = new Project("MyProject", Guid.NewGuid()),
                EndTime = DateTime.Parse("27.10.2023 20:45")
            };

            _sut = new EditableRecordViewModel(
                    _record,
                    _mockRecordRepository.Object,
                    _mockNavigationService.Object,
                    _mockNavigationService.Object,
                    null,
                    null,
                    _validator);
        }

        [Fact]
        public void UpdateRecord_WhenPropertiesIsCorrect()
        {
            var expectedDate = DateOnly.Parse("02.01.2024");
            var expectedStartTime = DateTime.Parse("02.01.2024 12:30");
            var expectedEndTime = DateTime.Parse("02.01.2024 20:30");
            var expectedTitle = "NewTitle";
            var expectedProject = new Project("newProject", Guid.NewGuid());
            
            _mockRecordRepository
                .Setup(x=> x.UpdateItem(It.IsAny<Models.Record>()))
                .Callback<Models.Record>(x=> Assert.Equal(expectedDate, x.Date));
            _sut.Date = expectedDate.ToString();

            _mockRecordRepository
                .Setup(x => x.UpdateItem(It.IsAny<Models.Record>()))
                .Callback<Models.Record>(x => Assert.Equal(expectedEndTime, x.EndTime));
            _sut.EndTime = expectedEndTime.ToString();

            _mockRecordRepository
                .Setup(x => x.UpdateItem(It.IsAny<Models.Record>()))
                .Callback<Models.Record>(x => Assert.Equal(expectedStartTime, x.StartTime));
            _sut.StartTime = expectedStartTime.ToString();

            _mockRecordRepository
                .Setup(x => x.UpdateItem(It.IsAny<Models.Record>()))
                .Callback<Models.Record>(x => Assert.Equal(expectedTitle, x.Title));
            _sut.Title = expectedTitle;

            _mockRecordRepository
                .Setup(x => x.UpdateItem(It.IsAny<Models.Record>()))
                .Callback<Models.Record>(x => Assert.Equal(expectedProject, x.Project));
            _sut.Project = expectedProject;

            _mockRecordRepository
                .Verify(x => x.UpdateItem(It.IsAny<Models.Record>()), Times.Exactly(5));

        }

        [Fact]
        public void IngoreUpdate_OnInvalidProperties()
        {
            _sut.StartTime = "";
            _sut.EndTime = "";
            _sut.Date = "";

            _mockRecordRepository
                .Verify(x => x.UpdateItem(It.IsAny<Models.Record>()), Times.Never);
        }

        [Fact]
        public void RemovesRecordFromRepository()
        {
            _mockRecordRepository
                .Setup(x => x.RemoveItem(It.IsAny<Models.Record>()))
                .Callback<Models.Record>(x => Assert.Equal(_sut.Record, x));
            _sut.RemoveRecordCommand.Execute(null);

            _mockRecordRepository
                .Verify(x => x.RemoveItem(It.IsAny<Models.Record>()), Times.Once);
        }



        //    [Fact]
        //    public void EditableRecord_UpdateCorrectFields()
        //    {

        //        _recordRepository.AddItem(_record);
        //        var expectedStartTime = DateTime.Parse("26.10.2023 10:00");
        //        var expectedEndTime = DateTime.Parse("26.10.2023 17:45");
        //        var expectedDate = DateOnly.Parse("26.10.2023");
        //        var expectedTilte = "NewTitle";
        //        var expectedProject = new Project("MySecondProject", Guid.NewGuid());

        //        _sut.Title = expectedTilte;
        //        _sut.StartTime = expectedStartTime.ToString();
        //        _sut.EndTime = expectedEndTime.ToString();
        //        _sut.Date = expectedDate.ToString();
        //        _sut.Project = expectedProject;

        //        var updatedRecord = _recordRepository.GetRecords(new FilterListingArgs()).First();

        //        Assert.Equal(expectedTilte, updatedRecord.Title);
        //        Assert.Equal(expectedStartTime, updatedRecord.StartTime);
        //        Assert.Equal(expectedEndTime, updatedRecord.EndTime);
        //        Assert.Equal(expectedDate, updatedRecord.Date);
        //        Assert.Equal(expectedProject, updatedRecord.Project);
        //    }

        //    [Fact]
        //    public void EditableRecord_IgnoreUpdateIncorrectFields()
        //    {
        //        _recordRepository.AddItem(_record);

        //        _sut.StartTime = "wrong";
        //        _sut.EndTime = "wrong";
        //        _sut.Date = "wrong";

        //        var updatedRecord = _recordRepository.GetItems(new FilterListingArgs()).First();

        //        Assert.Equal(_record.StartTime, updatedRecord.StartTime);
        //        Assert.Equal(_record.EndTime, updatedRecord.EndTime);
        //        Assert.Equal(_record.Date, updatedRecord.Date);

        //    }

        //    [Fact]
        //    public void DeleteRecord_DeleteRecordFromRepository()
        //    {
        //        _recordRepository.AddItem(_record);

        //        Assert.True(_recordRepository.Contains(_record));

        //        _sut.DeleteRecord();

        //        Assert.False(_recordRepository.Contains(_record));
        //    }

        //}
    }
}
