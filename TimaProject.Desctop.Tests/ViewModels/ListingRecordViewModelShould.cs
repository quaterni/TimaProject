using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Desctop.ViewModels;
using Xunit;

namespace TimaProject.Desctop.Tests.ViewModels
{
    public class ListingRecordViewModelShould
    {
        private readonly ListingRecordViewModel _sut;

        private readonly Mock<IRecordViewModelFactory> _mockFactory;

        private readonly Mock<IRecordService> _mockRecordService;
        private readonly Mock<IDateService> _mockDateService;

        public ListingRecordViewModelShould()
        {
            _mockFactory = new Mock<IRecordViewModelFactory>();
            _mockRecordService = new Mock<IRecordService>();
            _mockDateService = new Mock<IDateService>();

            _sut = new ListingRecordViewModel(
                _mockFactory.Object,
                _mockRecordService.Object,
                _mockDateService.Object);
        }

        [Fact]
        public void Records_LazyInit()
        {
            _ = _sut.Records;
            _ = _sut.Records;

            _mockRecordService.Verify(s => s.GetRecords(), Times.Once);
        }
    }
}
