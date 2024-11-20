using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using TimaProject.Desctop.Interfaces.Services;

namespace TimaProject.Desctop.Services
{
    internal class TimerExecutor : ITimerExecutor
    {
        private readonly CancellationTokenSource _cts;

        public DateTime StartTime
        { get; set; }

        public event EventHandler<TimeSpan>? Tick;

        public TimerExecutor()
        {
            _cts = new CancellationTokenSource();
        }


        public DateTime CurrentTime()
        {
            return DateTime.Now;
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }

        public void Start()
        {
            Task.Run(() =>
            {
                Observable
                .Timer(StartTime, TimeSpan.FromMilliseconds(200))
                .Subscribe(ticks =>
                {
                   Tick?.Invoke(this, DateTime.Now - StartTime);
                });
            },
            _cts.Token);
        }

    }
}
