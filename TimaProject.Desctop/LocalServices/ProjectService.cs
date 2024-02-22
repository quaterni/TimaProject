using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Services;

namespace TimaProject.Desctop.LocalServices
{
    internal class ProjectService : IProjectService
    {
        public ProjectService()
        {
            
        }

        public void AddProject(ProjectDTO project)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProjectDTO> GetProjects()
        {
            throw new NotImplementedException();
        }

        public bool IsProjectNameExisting(string name)
        {
            throw new NotImplementedException();
        }
    }
}
