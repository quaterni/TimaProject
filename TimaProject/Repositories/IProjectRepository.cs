using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    public interface IProjectRepository
    {
        public bool Contains(string name);

        public bool Contains(Project project);

        public bool Contains(int id);

        public int GetId();

        public void AddProject(Project project);

        public void UpdateProject(Project project);

        public bool RemoveProject(Project project);

        public List<Project> GetAllProjects();
    }
}
