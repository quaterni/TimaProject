using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TimaProject.Views.Controls
{


    public partial class TimerControl : UserControl
    {
        private const int  MILISECONDS_INTERVAL = 50;

        public static readonly DependencyProperty StartingTimeProperty =
            DependencyProperty.Register("StartingTime", typeof(DateTimeOffset?), typeof(TimerControl), new PropertyMetadata(null, StartingTimePropertyChanged));

        public static readonly DependencyPropertyKey IsActivePropertyKey =
            DependencyProperty.RegisterReadOnly("IsActive", typeof(bool), typeof(TimerControl), new PropertyMetadata(false));

        public static readonly DependencyProperty IsActiveProperty = IsActivePropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey TimePropertyKey =
            DependencyProperty.RegisterReadOnly("Time", typeof(TimeSpan), typeof(TimerControl), new PropertyMetadata(new TimeSpan(0)));

        public static readonly DependencyProperty TimeProperty = TimePropertyKey.DependencyProperty;

        public static readonly RoutedEvent TimeStartedEvent = 
            EventManager.RegisterRoutedEvent("TimeStarted", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTimeOffset?>), typeof(TimerControl));

        public static readonly RoutedEvent TimeStoppedEvent =
    EventManager.RegisterRoutedEvent("TimeStopped", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Tuple<DateTimeOffset?, DateTimeOffset?>>), typeof(TimerControl));

        private DateTimeOffset? _startingTime;

        private bool _isActive;

        private DispatcherTimer _timer;

        public DateTimeOffset? StartingTime
        {
            get { return (DateTimeOffset?)GetValue(StartingTimeProperty); }
            set { 
                _startingTime = value;
                SetValue(StartingTimeProperty, value);
            }
        }

        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            protected set
            {
                _isActive = value;
                SetValue(IsActivePropertyKey, value);
            }
        }

        public TimeSpan Time
        {
            get
            {
                return (TimeSpan)GetValue(TimeProperty);
            }
            protected set
            {
                SetValue(TimePropertyKey, value);
            }
        }

        public event RoutedPropertyChangedEventHandler<DateTimeOffset?> TimeStarted
        {
            add
            {
                AddHandler(TimeStartedEvent, value);
            }
            remove
            {
                RemoveHandler(TimeStartedEvent, value);
            }
        }

        public event RoutedPropertyChangedEventHandler<Tuple<DateTimeOffset, DateTimeOffset?>> TimeStopped
        {
            add
            { 
                AddHandler(TimeStoppedEvent, value);
            }
            remove
            {
                RemoveHandler(TimeStoppedEvent, value);
            }
        }

        private static void StartingTimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not TimerControl timer)
            {
                throw new ArgumentException("DependencyObject is not TimerControl");
            }
            timer.IsActive = e.NewValue is not null;
        }

        public TimerControl()
        {
            InitializeComponent();
        }

        protected void OnStartingTime(DateTimeOffset? startingTime)
        {
            var oldValue = StartingTime;
            StartingTime = startingTime;
           // Time = _isActive ? (TimeSpan)(DateTimeOffset.Now - _startingTime) : new TimeSpan(0);
            _timer = new DispatcherTimer();
            _timer.Tick += ChangeTime;
            _timer.Interval = new TimeSpan(0,0,0,0, MILISECONDS_INTERVAL);
            _timer.Start();
            RaiseEvent(new RoutedPropertyChangedEventArgs<DateTimeOffset?>(oldValue, startingTime, TimeStartedEvent));
        }

        private void ChangeTime(object? sender, EventArgs e)
        {
            Time = _isActive ? (TimeSpan)(DateTimeOffset.Now - _startingTime) : new TimeSpan(0);
        }

        protected void OnStoppingTime(DateTimeOffset stoppingTime)
        {
            Tuple<DateTimeOffset?, DateTimeOffset?> tuple = new Tuple<DateTimeOffset?, DateTimeOffset?>(StartingTime, stoppingTime);
            _timer.Stop();
            _timer.Tick -= ChangeTime;
            StartingTime = null;
            Time = new TimeSpan(0);
            RaiseEvent(new RoutedPropertyChangedEventArgs<Tuple<DateTimeOffset?, DateTimeOffset?>>(null, tuple, TimeStoppedEvent));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsActive)
            {
                OnStoppingTime(DateTimeOffset.Now);
            }
            else
            {
                OnStartingTime(DateTimeOffset.Now);
            }
        }

    }
}
