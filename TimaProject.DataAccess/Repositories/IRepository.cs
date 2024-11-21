using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.DataAccess.Repositories
{
    public interface IRepository<T>
    {
        void AddItem(T item);
        void UpdateItem(T item);
        bool RemoveItem(T item);
        bool Contains(T item);
        IEnumerable<T> GetItems(Func<T, bool> filterPredicate);

        event EventHandler<RepositoryChangedEventArgs<T>>? RepositoryChanged;
    }
}
