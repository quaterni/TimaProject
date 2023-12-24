using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.ViewModels.Containers
{
    public class ProjectContainerViewModel : ViewModelBase
    {
		private EditableProjectViewModel? _item;

		public EditableProjectViewModel? Item
		{
			get { return _item; }
			set { SetValue(ref _item, value); }
		}

		private bool _isSelected;

		public bool IsSelected
		{
			get { return _isSelected; }
		}

		private bool _isEmpty;

		public bool IsEmpty
		{
			get { return _isEmpty; }
		}


        public ProjectContainerViewModel(EditableProjectViewModel? item, bool isEmpty, bool isSelected)
        {
			_item = item;
			_isEmpty = isEmpty;
			_isSelected = isSelected;
        }
    }
}
