using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TimaProject.Desctop.Interfaces.ViewModels;
using TimaProject.Desctop.ViewModels;

namespace TimaProject.Desctop.Views
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

   

        private void OnClose(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ITimerViewModel.IsProjectFormOpened)
                && !((ITimerViewModel)DataContext).IsProjectFormOpened)
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
        }

        public void OnDialogClosed(object sender, DialogClosedEventArgs eventArgs)
        {
            ((ITimerViewModel)DataContext).IsProjectFormOpened = false;
            ((ITimerViewModel)DataContext).PropertyChanged -= OnClose;

        }

        public void OnDialogOpened(object sender, DialogOpenedEventArgs eventArgs)
        {
            ((ITimerViewModel)DataContext).OpenProjectFormCommand.Execute(this);
            ((ITimerViewModel)DataContext).PropertyChanged += OnClose;
        }

    }
}
