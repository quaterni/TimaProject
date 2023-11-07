using FluentValidation;
using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.ViewModels
{
    public class ProjectEditViewModel : ProjectViewModelBase
    {

        public ProjectEditViewModel(AbstractValidator<ProjectViewModelBase> validator) :base(validator)
        {
        }

    }
}
