﻿using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.ViewModels;

namespace TimaProject.Commands
{
    internal class TimerCommand : CommandBase
    {
        private readonly TimerViewModel _timerViewModel;

        public TimerCommand(TimerViewModel timerViewModel)
        {
            _timerViewModel = timerViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (_timerViewModel.State == TimerState.Running)
            {
                _timerViewModel.OnEndingTime();
            }
            else
            {
                _timerViewModel.OnStartingTime();
            }
        }
    }
}