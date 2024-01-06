using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.ViewModels
{
    public enum TimeSolverState
    {
        NotSolving,
        Solving
    }

    public class TimeSolver
    {
        private readonly ITimeBase _source;
        private readonly AbstractValidator<ITimeBase> _timeValidator;
        private TimeSolverState _state;

        public TimeSolver(ITimeBase source, AbstractValidator<ITimeBase> timeValidator)
        {
            _source = source;
            _timeValidator = timeValidator;
            _state = TimeSolverState.NotSolving;
        }


        public void Solve(string propertyName)
        {
            if(_state == TimeSolverState.Solving)
            {
                return;
            }
            var validationResult = _timeValidator.Validate(_source).ToDictionary(); 
            if(validationResult.ContainsKey(propertyName))
            {
                return;
            }

            switch(propertyName)
            {
                case nameof(ITimeBase.StartTime):
                    if(validationResult.ContainsKey(nameof(ITimeBase.EndTime)))
                    {
                        return;
                    }
                    SetTime();
                    break;
                case nameof(ITimeBase.EndTime):
                    if (validationResult.ContainsKey(nameof(ITimeBase.StartTime)))
                    {
                        return;
                    }
                    SetTime();
                    break;
                case nameof(ITimeBase.Time):
                    if (validationResult.ContainsKey(nameof(ITimeBase.EndTime)))
                    {
                        return;
                    }
                    SetStartTime();
                    break;
            }

        }

        private void SetTime()
        {
            _state = TimeSolverState.Solving;
            _source.Time = (DateTime.Parse(_source.EndTime) - DateTime.Parse(_source.StartTime)).ToString();
            _state = TimeSolverState.NotSolving;
        }

        private void SetStartTime()
        {
            _state = TimeSolverState.Solving;
            _source.StartTime = (DateTime.Parse(_source.EndTime) - TimeSpan.Parse(_source.Time)).ToString();
            _state = TimeSolverState.NotSolving;
        }
    }
}
