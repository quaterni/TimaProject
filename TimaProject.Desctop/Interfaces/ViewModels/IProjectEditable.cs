using System.Windows.Input;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    internal interface IProjectEditable
    {
        ICommand OpenProjectCommand { get; }
        IProjectFormViewModel ProjectFormViewModel { get; }
        bool IsProjectFormOpened { get; }
    }
}