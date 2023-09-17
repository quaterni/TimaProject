using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected virtual void SetValue<TValue>(ref TValue member, TValue value, [CallerMemberName]string propertyName = null!)
        {
            if (object.Equals(member, value)) 
                return;
            member = value;
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
           
        }
    }
}
