using Xunit;
using TimaProject.Desctop.ViewModels;
using Moq;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using FluentAssertions;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.Interfaces.Services;

namespace TimaProject.Desctop.Tests.ViewModels
{
    public class RecordViewModelShould
    {
        
        private static RecordDto s_record
        {
            get
            {
                return new RecordDto("2024-02-15T10:45", "MyRecord", new Guid("588a836c-ec2c-4489-9a76-760bb99fedd6"))
                {
                    Date = "2024-02-15",
                    EndTime = "2024-02-15T16:11:49",
                    Time = "5:26:49",
                    ProjectName = "FirstProject",
                    ProjectId = new Guid("80e680ac-7b06-46b6-bbed-39c7232d9896"),
                    IsActive = false
                };
            }
        }

        private readonly RecordViewModel _sut;

        private readonly Mock<IAddNoteFormFactory> _mockAddNoteFormFactory;
        private readonly Mock<IListingNoteViewModelFactory> _mockListingNoteFactory;
        private readonly Mock<IRecordService> _mockRecordService;


        public RecordViewModelShould()
        {
            _mockAddNoteFormFactory = new();
            _mockListingNoteFactory = new();
            _mockRecordService = new();
            _sut = new RecordViewModel(
                    s_record,
                    new Mock<ITimeFormViewModelFactory>().Object,
                    new Mock<IProjectFormViewModelFactory>().Object,
                    _mockAddNoteFormFactory.Object,
                    _mockListingNoteFactory.Object,
                    _mockRecordService.Object);
            _mockAddNoteFormFactory
                .Setup(s => s.Create(It.IsAny<Guid>()))
                .Returns(new Mock<IAddNoteFormViewModel>().Object);

            _mockListingNoteFactory
                .Setup(s => s.Create(It.IsAny<Guid>()))
                .Returns(new Mock<IListingNoteViewModel>().Object);

        }

        [Fact]
        public void Init()
        {
            _sut.StartTime.Should().Be(s_record.StartTime);
            _sut.EndTime.Should().Be(s_record.EndTime);
            _sut.Time.Should().Be(s_record.Time);
            _sut.Date.Should().Be(s_record.Date);
            _sut.Id.Should().Be(s_record.RecordId);
            _sut.ProjectId.Should().Be(s_record.ProjectId);
            _sut.ProjectName.Should().Be(s_record.ProjectName);

            _sut.DeleteRecordCommand.Should().NotBeNull();
            _sut.IsNoteExpanded.Should().BeFalse();
        }

        [Fact]
        public void AddNoteFormViewModel_LazyInit()
        {
            var addNote1 = _sut.AddNoteViewModel;
            var addNote2 = _sut.AddNoteViewModel;
            addNote1.Should().Be(addNote2);
            _mockAddNoteFormFactory.Verify(s => s.Create(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void ListingNoteViewModel_LazyInit()
        {
            var listingNote1 = _sut.ListingNoteViewModel;
            var listingNote2 = _sut.ListingNoteViewModel;
            listingNote1.Should().Be(listingNote2);
            _mockListingNoteFactory.Verify(s => s.Create(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void DeleteRecordCommand_WhenExecuted_DeletesRecord()
        {
            _sut.DeleteRecordCommand.Execute(null);

            _mockRecordService
                .Verify(s => s.DeleteRecord(s_record.RecordId), Times.Once);
        }
    }
}
