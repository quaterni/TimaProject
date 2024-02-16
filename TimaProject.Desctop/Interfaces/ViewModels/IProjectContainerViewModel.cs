using System;

namespace TimaProject.Desctop.Interfaces.ViewModels
{
    public interface IProjectContainerViewModel
    {
        string Name { get; }
        Guid Id { get; }
        bool IsSelected { get; }
        bool IsEmpty { get; }
    }
}