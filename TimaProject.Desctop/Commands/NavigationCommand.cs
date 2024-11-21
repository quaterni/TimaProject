using MvvmTools.Base;
using MvvmTools.Navigation.Services;


namespace TimaProject.Desctop.Commands
{
    public class NavigationCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public NavigationCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate(parameter);
        }
    }
}
