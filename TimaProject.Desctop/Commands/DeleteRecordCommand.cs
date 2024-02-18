using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.ViewModels;

namespace TimaProject.Desctop.Commands
{
    internal class DeleteRecordCommand : CommandBase
    {
        private readonly RecordViewModel _editableRecordViewModel;

        public DeleteRecordCommand(RecordViewModel editableRecordViewModel)
        {
            _editableRecordViewModel = editableRecordViewModel;
        }

        public override void Execute(object? parameter)
        {
            _editableRecordViewModel.RemoveRecord();
        }
    }
}
