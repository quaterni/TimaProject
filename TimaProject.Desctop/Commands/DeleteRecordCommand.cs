﻿using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.ViewModels;

namespace TimaProject.Desctop.Commands
{
    public class DeleteRecordCommand : CommandBase
    {
        private readonly EditableRecordViewModel _editableRecordViewModel;

        public DeleteRecordCommand(EditableRecordViewModel editableRecordViewModel)
        {
            _editableRecordViewModel = editableRecordViewModel;
        }

        public override void Execute(object? parameter)
        {
            _editableRecordViewModel.RemoveRecord();
        }
    }
}