using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.ViewModels;
using Xunit;

namespace TimaProject.Desctop.Tests.ViewModels
{
    public class AddNoteFormViewModelShould
    {
        private readonly Mock<INoteService> _mockNoteService;
        private readonly AddNoteFormViewModel _sut;

        private static Guid s_recordId = new Guid("80e680ac-7b06-46b6-bbed-39c7232d9896");

        public AddNoteFormViewModelShould()
        {
            _mockNoteService = new Mock<INoteService>();
            _sut = new AddNoteFormViewModel(s_recordId, _mockNoteService.Object);
        }

        [Fact]
        public void Init()
        {
            _sut.Text.Should().BeEmpty();
            _sut.CanAdd.Should().BeFalse();
        }

        [Fact]
        public void CanAdd_WhenTextNotEmpty_True()
        {
            _sut.Text = new Fixture().Create<string>();

            _sut.CanAdd.Should().BeTrue();
        }

        [Fact]
        public void CanAdd_WhenTextChangedToEmtpy_False()
        {
            _sut.Text = new Fixture().Create<string>();

            _sut.Text = string.Empty;

            _sut.CanAdd.Should().BeFalse();
        }

        [Fact]
        public void AddNoteCommand_WhenExecutes_AddNote()
        {
            _mockNoteService
                .Setup(s => s.Add(It.IsAny<NoteDTO>()))
                .Callback<NoteDTO>(dto =>
                {
                    dto.RecordId.Should().Be(s_recordId);
                    dto.Text.Should().Be(_sut.Text);
                });
            _sut.Text = new Fixture().Create<string>();

            _sut.AddNoteCommand.Execute(null);

            _mockNoteService.Verify(s => s.Add(It.IsAny<NoteDTO>()));
        }
    }
}
