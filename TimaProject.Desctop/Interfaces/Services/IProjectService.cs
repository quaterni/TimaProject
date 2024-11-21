using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;

namespace TimaProject.Desctop.Interfaces.Services
{
    internal interface IProjectService
    {
        void AddProject(ProjectDTO project);
        bool IsProjectNameExisting(string name);
        IEnumerable<ProjectDTO> GetProjects();
    }
}
