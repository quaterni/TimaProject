using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly List<Project> _projects;

        private int _currentId;

        public ProjectRepository()
        {
            _projects = new List<Project>();
            _currentId = 1;
        }

        public void AddProject(Project project)
        {
            if (Contains(project.Id))
            {
                throw new Exception("Project with same id exists");
            }
            if (Contains(project.Name))
            {
                throw new Exception("Project with same name exists");
            }
            _projects.Add(project);
            _currentId++;
        }

        public bool Contains(string name)
        {
            return _projects.Find(p => p.Name.Equals(name)) is not null;
        }

        public bool Contains(Project project)
        {
            return _projects.Contains(project);
        }

        public bool Contains(int id)
        {
            return _projects.Find(p => p.Id == id) is not null;
        }

        public List<Project> GetAllProjects()
        {
            return _projects;
        }

        public int GetId()
        {
            return _currentId;
        }

        public bool RemoveProject(Project project)
        {
            return _projects.Remove(project);
        }

        public void UpdateProject(Project project)
        {
            var oldProject = _projects.Find(r => r.Id == project.Id);
            if(oldProject == null)
            {
                throw new Exception("Project not found");
            }
            _projects.Remove(oldProject);
            _projects.Add(project);
        }
    }
}
