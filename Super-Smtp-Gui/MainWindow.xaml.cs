using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SuperSmtpGui.SMTP;

namespace SuperSmtpGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SmtpViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = (SmtpViewModel)this.DataContext;
            viewModel.Start();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.Stop();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                buttonOnOff.Content = "Stop Server";
                viewModel.Start();
            }
        }

        private void buttonOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                buttonOnOff.Content = "Start Server";
                viewModel.Stop();
            }
        }
    }
}
