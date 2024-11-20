using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.ViewModels.Containers
{
    public class ProjectContainerViewModel : IProjectContainerViewModel
    {

        public bool IsSelected
        {
            get;
        }

        public bool IsEmpty
        {
            get;
        }

        public string Name { get; }

        public Guid Id { get; }

        public ProjectContainerViewModel(ProjectDTO project, bool isSelected)
        {
            Name = project.Name;
            Id = project.Id;
            IsEmpty = false;
            IsSelected = isSelected;
        }

        public ProjectContainerViewModel(bool isSelected)
        {
            Name = string.Empty;
            Id = Guid.Empty;
            IsEmpty = true;
            IsSelected = isSelected;
        }
    }
}
