using CommunityToolkit.Mvvm.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public record struct DateContainer(DateOnly Date, TimeSpan Hours);

    public interface IListingRecordViewModel
    {
        public ObservableGroupedCollection<DateContainer, IRecordViewModel> Records { get; }
    }
}
