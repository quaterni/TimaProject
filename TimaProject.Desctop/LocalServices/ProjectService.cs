using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;
using TimaProject.Desctop.Interfaces.Services;
using TimaProject.Domain.Models;
using TimaProject.LocalController;

namespace TimaProject.Desctop.LocalServices
{
    internal class ProjectService : IProjectService
    {
        private readonly ProjectController _controller;

        public ProjectService()
        {
            _controller = new ProjectController();
        }

        public void AddProject(ProjectDTO project)
        {
            _controller.AddProject(FromTo(project));
        }

        public IEnumerable<ProjectDTO> GetProjects()
        {
            return _controller.GetProjects().Select(p=> ToFrom(p));
        }

        public bool IsProjectNameExisting(string name)
        {
            return _controller.IsProjectNameExisting(name);
        }

        private Project FromTo(ProjectDTO project)
        {
            return new Project(project.Name, project.Id);
        }

        private ProjectDTO ToFrom(Project project)
        {
            return new ProjectDTO(project.Name, project.Id, project.Id.Equals(Guid.Empty));
        }
    }
}
