using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Factories;
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.ViewModels.Containers;

namespace TimaProject.Desctop.ViewModels.Factories
{
    internal class ProjectContainerFactory : IProjectContainerFactory
    {
        public IProjectContainerViewModel Create(ProjectDTO projectDTO, bool isSelected = false)
        {
            return new ProjectContainerViewModel(projectDTO, isSelected);
        }

        public IProjectContainerViewModel CreateEmpty(bool isSelected = false)
        {
            return new ProjectContainerViewModel(isSelected);
        }
    }
}
