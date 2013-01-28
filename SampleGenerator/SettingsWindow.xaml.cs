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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        private AnnealingSettingsProvider settings;

        internal AnnealingSettingsProvider Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        public SettingsWindow()
        {
            InitializeComponent();
        }
        /*
        public int MaxAnnealingIterations
        {
            get { return int.Parse(MaxAnnealingItTextBox.Text); }
        }

        public int InnerAnnealingIterations
        {
            get { return int.Parse(InnerAnnealingItTextBox.Text); }
        }

        public double StartingTemperature
        {
            get { return double.Parse(InitTemperatureTextBox.Text); }
        }

        public double CoolingFactor
        {
            get { return double.Parse(CoolingFactorTextBox.Text); }
        }
        */

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                settings = new AnnealingSettingsProvider();
                settings.MaxAnnealingIterations = int.Parse(MaxAnnealingEditor.TextBoxText);
                settings.InnerAnnealingIterations = int.Parse(InnerAnnealingEditor.TextBoxText);
                settings.StartingTemperature = double.Parse(StartingTemperatureEditor.TextBoxText);
                settings.CoolingFactor = double.Parse(CoolingFactorEditor.TextBoxText);

                this.DialogResult = true;

                return;
            }

            catch (FormatException ex)
            {
                MainWindow.showMessage("Invalid data!\n" + ex.Message);
            }
            catch (Exception ex)
            {
                MainWindow.showMessage("Unknown error!\n" + ex.StackTrace);
            }

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
