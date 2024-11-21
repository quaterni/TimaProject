using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.ViewModels;

namespace TimaProject.Desctop.Interfaces.Factories
{
    internal interface IProjectContainerFactory
    {
        IProjectContainerViewModel Create(ProjectDTO projectDTO, bool isSelected = false);
        IProjectContainerViewModel CreateEmpty(bool isSelected = false);
    }
}
