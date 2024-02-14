namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface IProjectContainerViewModel
    {
        string Name { get; }
        string Id { get; }
        bool IsSelected { get; }
        bool IsEmpty { get; }
    }
}