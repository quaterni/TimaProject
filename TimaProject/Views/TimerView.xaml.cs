using System;
using System.Collections.Generic;
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
using TimaProject.ViewModels;

namespace TimaProject.Views
{
    /// <summary>
    /// Interaction logic for TimerView.xaml
    /// </summary>
    public partial class TimerView : UserControl
    {
        public TimerView()
        {
            InitializeComponent();
        }

        private void TimerControl_TimeStarted(object sender, RoutedPropertyChangedEventArgs<DateTime> e)
        {
            if(DataContext is TimerViewModel vm)
            {
                vm.OnStartingTime();
            }
        }

        private void TimerControl_TimeStopped(object sender, RoutedPropertyChangedEventArgs<Tuple<DateTime, DateTime>> e)
        {
            if (DataContext is TimerViewModel vm)
            {
                vm.OnEndingTime();
            }
        }
    }
}
