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
using System.Windows.Shapes;

namespace SampleGen
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        public string TimeText
        {
            set
            {
                this.timeLabel.Content = value;
            }
        }
        public string ProgressText
        {
            set
            {
                this.progressLabel.Content = value;
            }
        }
        public int ProgressValue
        {
            set
            {
                this.progressBar.Value = value;
            }
        }
        public event EventHandler Cancel = delegate { };
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Cancel(sender, e);
        }

        private void closeWindow(object sender, EventArgs e)
        {
            Cancel(sender, e);
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
