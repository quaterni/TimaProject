using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimaProject.Exceptions;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly List<Project> _projects;

        public ProjectRepository()
        {
            _projects = new List<Project>
            {
                Project.Empty
            };
        }

        public event EventHandler<RepositoryChangedEventArgs<Project>>? RepositoryChanged;

        public bool Contains(string name)
        {
            return _projects.Find(p => p.Name.Equals(name)) is not null;
        }

        private void OnRepositoryChanged(Project item, RepositoryChangedOperation operation)
        {
            RepositoryChanged?.Invoke(this, new RepositoryChangedEventArgs<Project>(item, operation));
        }

        public void AddItem(Project item)
        {
            if (Contains(item))
            {
                throw new AddingNotUniqueItem("Project with same id exists");
            }
            if (Contains(item.Name))
            {
                throw new AddingNotUniqueItem("Project with same name exists");
            }
            _projects.Add(item);
            OnRepositoryChanged(item, RepositoryChangedOperation.Add);
        }

        public void UpdateItem(Project item)
        {
            if (item.Id.Equals(Project.Empty.Id))
            {
                throw new ChangeEmptyProjectException();
            }
            Project oldProject = _projects.Find(r => r.Id == item.Id)
                ?? throw new NotFoundException<Project>(item, "Project doesn't contain in repository");

            _projects[_projects.IndexOf(oldProject)] = item;
            OnRepositoryChanged(item, RepositoryChangedOperation.Update);
        }

        public bool RemoveItem(Project item)
        {
            if(item.Equals(Project.Empty))
            {
                throw new ChangeEmptyProjectException();
            }

            var result = _projects.Remove(item);
            if (result)
            {
                OnRepositoryChanged(item, RepositoryChangedOperation.Remove);
            }
            return result;
        }

        public bool Contains(Project item)
        {
            return _projects.Contains(item);
        }

        public IEnumerable<Project> GetItems(Func<Project, bool> filterPredicate)
        {
           return _projects.Where((project)=> filterPredicate(project) && !project.Equals(Project.Empty));
        }
    }
}
