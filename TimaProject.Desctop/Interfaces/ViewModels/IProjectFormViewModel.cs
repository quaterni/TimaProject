using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface IProjectFormViewModel
    {
        ObservableCollection<IProjectContainerViewModel> Projects { get; }

        string NewProjectName { get; set; }

        bool CanAddNewProject { get; }

        ICommand AddNewProjectCommand { get; }
    }
}