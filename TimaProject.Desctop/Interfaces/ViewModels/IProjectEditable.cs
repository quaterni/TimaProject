using System.Windows.Input;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    internal interface IProjectEditable
    {
        ICommand OpenProjectFormCommand { get; }
        IProjectFormViewModel? ProjectFormViewModel { get; }
        bool IsProjectFormOpened { get; }
    }
}