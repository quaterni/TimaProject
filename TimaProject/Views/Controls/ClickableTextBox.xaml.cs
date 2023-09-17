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

namespace TimaProject.Views.Controls
{
    /// <summary>
    /// Interaction logic for ClickableTextBox.xaml
    /// </summary>
    public partial class ClickableTextBox : UserControl
    {


        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(ClickableTextBox), new PropertyMetadata(false));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ClickableTextBox), new PropertyMetadata(string.Empty));

        public string SuggestingText
        {
            get { return (string)GetValue(SuggestingTextProperty); }
            set { SetValue(SuggestingTextProperty, value); }
        }

        public static readonly DependencyProperty SuggestingTextProperty =
            DependencyProperty.Register("SuggestingText", typeof(string), typeof(ClickableTextBox), new PropertyMetadata(string.Empty));

        public ClickableTextBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsEditing = true;
            tb.Focus();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsEditing = false;
        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            tb.CaretIndex = tb.Text.Length;
        }
    }
}
