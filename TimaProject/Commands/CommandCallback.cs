using MvvmTools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Commands
{
    internal class CommandCallback : CommandBase
    {
        private readonly Action<object?> _callback;

        public CommandCallback(Action<object?> callback)
        {
              _callback = callback;
        }

        public override void Execute(object? parameter)
        {
            _callback.Invoke(parameter);
        }
    }
}
