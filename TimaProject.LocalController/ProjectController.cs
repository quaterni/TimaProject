using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.DataAccess.Repositories;
using TimaProject.Domain.Models;

namespace TimaProject.LocalController
{

    public class ProjectController
    {
        private readonly ProjectRepository _repository;

        public ProjectController()
        {
            _repository = Store.GetProjectRepository();
        }

        public void AddProject(Project project)
        {
            _repository.AddItem(project);
        }

        public IEnumerable<Project> GetProjects()
        {
            return _repository.GetItems();
        }

        public bool IsProjectNameExisting(string name)
        {
            return _repository.Contains(name);
        }


    }
}
