using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TimaProject.Desctop.DTOs;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface IProjectFormViewModel : IDialog
    {
        ObservableCollection<IProjectContainerViewModel> Projects { get; }

        string NewProjectName { get; set; }

        bool CanAddNewProject { get; }

        ICommand AddNewProjectCommand { get; }

        event EventHandler<ProjectDTO> ProjectSelected;
    }
}