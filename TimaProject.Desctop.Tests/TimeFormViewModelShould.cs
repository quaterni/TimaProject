using Moq;
using MvvmTools.Navigation.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimaProject.Desctop.Exceptions;
using TimaProject.Domain.Models;
using TimaProject.Desctop.ViewModels;
using TimaProject.Desctop.ViewModels.Validators;
using Xunit;

namespace TimaProject.Desctop.Tests
{

    public class MockRecord : IRecordViewModel
    {
        public int SetStartTimeCounter = 0;
        public int SetEndTimeCounter = 0;
        public int SetTimeCounter = 0;
        public int SetDateCounter = 0;

        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Project Project { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private string _startTime;
        public string StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
                SetStartTimeCounter++;
            }
        }

        private string _endTime;

        public string EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
                SetEndTimeCounter++;
            }
        }

        private string _time;

        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                SetTimeCounter++;
            }
        }

        private string _date;

        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                SetDateCounter++;
            }
        }
    }

    public class TimeFormViewModelShould
    {
        private readonly TimeFormViewModel _sut;
        private readonly TimeValidator _validator;
        private readonly Mock<INavigationService> _mockNavigationService;
        private readonly static MockRecord _testRecord = new()
        {
            StartTime = "05.01.2024 10:00",
            EndTime = "05.01.2024 12:00",
            Time = "2:00",
            Date = "05.01.2024"
        };

        private readonly static MockRecord _mockRecord = new()
        {
            StartTime = "05.01.2024 10:00",
            EndTime = "05.01.2024 12:00",
            Time = "2:00",
            Date = "05.01.2024"
        };

        public TimeFormViewModelShould()
        {
            _mockNavigationService = new Mock<INavigationService>();
            _mockRecord.StartTime = "05.01.2024 10:00";
            _mockRecord.EndTime = "05.01.2024 12:00";
            _mockRecord.Time = "2:00";
            _mockRecord.Date = "05.01.2024";

            _validator = new TimeValidator();
            _sut = new TimeFormViewModel(
                _mockRecord,
                _validator,
                _mockNavigationService.Object);
        }

        [Fact]
        public void GetValuesFromRecord_AfterInits()
        {

            Assert.Equal(_mockRecord.StartTime, _sut.StartTime);
            Assert.Equal(_mockRecord.EndTime, _sut.EndTime);
            Assert.Equal(_mockRecord.Time, _sut.Time);
            Assert.Equal(_mockRecord.Date, _sut.Date);
        }

        [Fact]
        public void NotGetValues_IfTimeFormNotValid()
        {
            _mockRecord.SetStartTimeCounter = 0;
            _mockRecord.SetTimeCounter = 0;
            _mockRecord.SetEndTimeCounter = 0;
            _mockRecord.SetDateCounter = 0;

            var expected = new MockRecord()
            {
                StartTime = _mockRecord.StartTime,
                EndTime = _mockRecord.EndTime,
                Time = _mockRecord.Time,
                Date = _mockRecord.Date
            };

            _sut.StartTime = "";
            _sut.EndTime = "12.12.2023 10:00";
            _sut.Date = "12.12.2023";
            Assert.True(_sut.HasErrors);

            Assert.Equal(expected.StartTime, _mockRecord.StartTime);
            Assert.Equal(expected.EndTime, _mockRecord.EndTime);
            Assert.Equal(expected.Time, _mockRecord.Time);
            Assert.Equal(expected.Date, _mockRecord.Date);

            _sut.EndTime = "";
            _sut.StartTime = "12.12.2023 10:00";
            _sut.Date = "12.12.2023";
            Assert.True(_sut.HasErrors);

            Assert.Equal(expected.StartTime, _mockRecord.StartTime);
            Assert.Equal(expected.EndTime, _mockRecord.EndTime);
            Assert.Equal(expected.Time, _mockRecord.Time);
            Assert.Equal(expected.Date, _mockRecord.Date);

            _sut.Time = "";
            _sut.StartTime = "12.12.2023 10:00";
            _sut.Date = "12.12.2023";
            Assert.True(_sut.HasErrors);

            Assert.Equal(expected.StartTime, _mockRecord.StartTime);
            Assert.Equal(expected.EndTime, _mockRecord.EndTime);
            Assert.Equal(expected.Time, _mockRecord.Time);
            Assert.Equal(expected.Date, _mockRecord.Date);

            _sut.Date = "";
            _sut.StartTime = "12.12.2023 10:00";
            _sut.EndTime = "12.12.2023 12:00";
            _sut.Time = "2:00";
            Assert.True(_sut.HasErrors);

            Assert.Equal(expected.StartTime, _mockRecord.StartTime);
            Assert.Equal(expected.EndTime, _mockRecord.EndTime);
            Assert.Equal(expected.Time, _mockRecord.Time);
            Assert.Equal(expected.Date, _mockRecord.Date);
        }

        [Theory, MemberData(nameof(StartTimeTestData))]
        public void SetStartTimeToSource(ITimeBase setupTimeBase, ITimeBase expected)
        {
            MockRecord mockRecord = new()
            {
                StartTime = "05.01.2024 10:00",
                EndTime = "05.01.2024 12:00",
                Time = "2:00",
                Date = "05.01.2024"
            };

            var sut = new TimeFormViewModel(
                mockRecord,
                _validator,
                _mockNavigationService.Object);

            sut.EndTime = setupTimeBase.EndTime;
            sut.Time = setupTimeBase.Time;
            sut.Date = setupTimeBase.Date;
            sut.StartTime = setupTimeBase.StartTime;

            Assert.Equal(expected.StartTime, mockRecord.StartTime);
            Assert.Equal(expected.EndTime, mockRecord.EndTime);
            Assert.Equal(expected.Time, mockRecord.Time);
            Assert.Equal(expected.Date, mockRecord.Date);
        }

        public static IEnumerable<object[]> StartTimeTestData()
        {
            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = "05.01.2024 12:00",
                    Time = "2:00",
                    Date = "05.01.2024",
                    StartTime = "05.01.2024 11:00"
                },
                new MockRecord()
                {
                    EndTime = "05.01.2024 12:00",
                    Date = "05.01.2024",
                    Time = "01:00:00",
                    StartTime = "05.01.2024 11:00"
                },
            };

            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = "05.01.2024 12:00",
                    Date = "05.01.2024",
                    Time = "",
                    StartTime = "05.01.2024 11:00"
                },
                new MockRecord()
                {
                    EndTime = "05.01.2024 12:00",
                    Date = "05.01.2024",
                    Time = "01:00:00",
                    StartTime = "05.01.2024 11:00"
                },
            };

            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = "05.01.2024 12:00",
                    Date = "05.01.2024",
                    Time = "",
                    StartTime = ""
                },
                new MockRecord()
                {
                    StartTime = "05.01.2024 10:00",
                    EndTime = "05.01.2024 12:00",
                    Time = "2:00",
                    Date = "05.01.2024"
                }
            };

            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = "",
                    Date = "05.01.2024",
                    Time = "1:00:00",
                    StartTime = "05.01.2024 11:00"
                },
                new MockRecord()
                {
                    StartTime = "05.01.2024 10:00",
                    EndTime = "05.01.2024 12:00",
                    Time = "2:00",
                    Date = "05.01.2024"
                }
            };

        }

        [Theory, MemberData(nameof(EndTimeTestData))]
        public void SetEndTimeToSource(ITimeBase setupTimeBase, ITimeBase expected)
        {
            MockRecord mockRecord = new()
            {
                StartTime = "05.01.2024 10:00",
                EndTime = "05.01.2024 12:00",
                Time = "2:00",
                Date = "05.01.2024"
            };

            var sut = new TimeFormViewModel(
                mockRecord,
                _validator,
                _mockNavigationService.Object);

            sut.Time = setupTimeBase.Time;
            sut.Date = setupTimeBase.Date;
            sut.StartTime = setupTimeBase.StartTime;
            sut.EndTime = setupTimeBase.EndTime;


            Assert.Equal(expected.StartTime, mockRecord.StartTime);
            Assert.Equal(expected.EndTime, mockRecord.EndTime);
            Assert.Equal(expected.Time, mockRecord.Time);
            Assert.Equal(expected.Date, mockRecord.Date);
        }

        public static IEnumerable<object[]> EndTimeTestData()
        {
            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = "05.01.2024 15:00",
                    Date = _testRecord.Date,
                    Time = _testRecord.Time,
                    StartTime = _testRecord.StartTime
                },
                new MockRecord()
                {
                    EndTime = "05.01.2024 15:00",
                    Date = _testRecord.Date,
                    Time = "05:00:00",
                    StartTime = _testRecord.StartTime
                },
            };

            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = "05.01.2024 15:00",
                    Date = _testRecord.Date,
                    Time = "",
                    StartTime = _testRecord.StartTime
                },
                new MockRecord()
                {
                    EndTime = "05.01.2024 15:00",
                    Date = _testRecord.Date,
                    Time = "05:00:00",
                    StartTime = _testRecord.StartTime
                },
            };

            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = "",
                    Date = _testRecord.Date,
                    Time = _testRecord.Time,
                    StartTime = _testRecord.StartTime
                },
                _testRecord
            };
            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = _testRecord.EndTime,
                    Date = _testRecord.Date,
                    Time = "",
                    StartTime = ""
                },
                _testRecord
            };

            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = "05.01.2024 15:00",
                    Date = _testRecord.Date,
                    Time = _testRecord.Time,
                    StartTime = ""
                },
                _testRecord
            };

            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = "05.01.2024 15:00",
                    Date = "",
                    Time = _testRecord.Time,
                    StartTime = _testRecord.StartTime
                },
                _testRecord
            };
        }

        [Theory, MemberData(nameof(TimeTestData))]
        public void SetTimeToSource(ITimeBase setupTimeBase, ITimeBase expected)
        {
            MockRecord mockRecord = new()
            {
                StartTime = "05.01.2024 10:00",
                EndTime = "05.01.2024 12:00",
                Time = "2:00",
                Date = "05.01.2024"
            };

            var sut = new TimeFormViewModel(
                mockRecord,
                _validator,
                _mockNavigationService.Object);

            sut.Date = setupTimeBase.Date;
            sut.StartTime = setupTimeBase.StartTime;
            sut.EndTime = setupTimeBase.EndTime;
            sut.Time = setupTimeBase.Time;


            Assert.Equal(expected.StartTime, mockRecord.StartTime);
            Assert.Equal(expected.EndTime, mockRecord.EndTime);
            Assert.Equal(expected.Time, mockRecord.Time);
            Assert.Equal(expected.Date, mockRecord.Date);
        }


        public static IEnumerable<object[]> TimeTestData()
        {
            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = _testRecord.EndTime,
                    Date = _testRecord.Date,
                    Time = "3:00:00",
                    StartTime = _testRecord.StartTime
                },
                new MockRecord()
                {
                    EndTime = _testRecord.EndTime,
                    Date = _testRecord.Date,
                    Time = "3:00:00",
                    StartTime = "05.01.2024 9:00:00"
                }
            };

            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = "",
                    Date = _testRecord.Date,
                    Time = "3:00:00",
                    StartTime = _testRecord.StartTime
                },
                new MockRecord()
                {
                StartTime = "05.01.2024 10:00",
                EndTime = "05.01.2024 12:00",
                Time = "2:00",
                Date = "05.01.2024"
                }
        };

            yield return new object[]
            {
                new MockRecord()
                {
                    EndTime = _testRecord.Time,
                    Date = "",
                    Time =  "3:00:00",
                    StartTime = _testRecord.StartTime
                },
                new MockRecord()
                {
                StartTime = "05.01.2024 10:00",
                EndTime = "05.01.2024 12:00",
                Time = "2:00",
                Date = "05.01.2024"
                }
            };

        }


        [Fact]
        public void SetCurrentTimeToEndTime_WhenEndTimeDisabled()
        {
            var sut = new TimeFormViewModel(
                _mockRecord,
                _validator,
                _mockNavigationService.Object,
                isEndTimeEnabled: false);

            Assert.True(DateTime.TryParse(sut.EndTime, out var result));
            Assert.Equal(DateTime.Now, result, TimeSpan.FromSeconds(1));

        }

        [Fact]
        public void ThrowException_WhenDisabledEndTimeSet()
        {
            var sut = new TimeFormViewModel(
                _mockRecord,
                _validator,
                _mockNavigationService.Object,
                isEndTimeEnabled: false);

            Assert.Throws<SettingDisableEndTimeException>(() => sut.EndTime = "01.01.2024 10:00");
        }

        [Fact]
        public void SetCorrectProperites_WhenDisabledEndTimeSet()
        {
            var sut = new TimeFormViewModel(
                _mockRecord,
                _validator,
                _mockNavigationService.Object,
                isEndTimeEnabled: false);
            var expectedStartTime = "05.01.2024 9:00:00";
            var expectedDate = "03.01.2024";

            sut.StartTime = expectedStartTime;
            sut.Date = expectedDate;
            Assert.Equal(expectedStartTime, _mockRecord.StartTime);
            Assert.Equal(expectedDate, _mockRecord.Date);

            var expectedTime = "5:00:00";
            sut.Time = expectedTime;
            Assert.Equal(expectedTime, _mockRecord.Time);

        }

    }
}
