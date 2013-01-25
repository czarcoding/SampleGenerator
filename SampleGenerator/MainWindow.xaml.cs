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
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace SampleGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ConsoleRedirectWriter consoleRedirectWriter = new ConsoleRedirectWriter();
        private String lastConsoleString;

        private static int digits = 16;
        public static string DOUBLE_FORMAT_STRING = "N" + digits.ToString();
        private string xRegex = "\\d+(\\.\\d+)?";

        private bool generateX = false;
        AnnealingSettingsProvider sett;
        //private Point x;

        private SampleGenerator sampleGenerator;

        public MainWindow()
        {
            InitializeComponent();

            consoleRedirectWriter.OnWrite += delegate(string value) { lastConsoleString = value; };
            Console.WriteLine("Console output redirected\n");
            DimensionTextBox.Focus();
        }

        private void consoleTextBox_Initialized(object sender, EventArgs e)
        {
            consoleRedirectWriter.OnWrite += delegate(string value)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    consoleTextBox.AppendText(value);
                    consoleTextBox.ScrollToEnd();
                }));
            };
        }

        private void generateSample()
        {
            int dimension = 0;
            int sampleCount = 0;
            double mean = 0;

            sampleGenerator = new SampleGenerator();
            sampleGenerator.setAnnealingSettings(this.sett);
            sampleGenerator.SampleArrayChanged += SampleChanged;

            try
            {
                dimension = int.Parse(DimensionTextBox.Text);
                sampleCount = int.Parse(SampleCountTextBox.Text);
                mean = double.Parse(MeanTextBox.Text);

                Point x = parseXInput(dimension);

                Object[] arguments = { sampleCount, dimension, mean, x };
                sampleGenerator.run(sampleCount, dimension, mean, x);
                sampleListView.ItemsSource = sampleGenerator.SampleArray;
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

        private void generateSampleButton_Click(object sender, RoutedEventArgs e)
        {
            generateSample();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                generateSample();
            }
        }

        public static void showMessage(string message)
        {
            Console.WriteLine(message);
            MessageBox.Show(message);
        }

        private void clearConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            consoleTextBox.Clear();
        }

        private Point parseXInput(int dim)
        {
            if (!generateX)
            {
                string text = this.XValuesTextBox.Text;

                if (text == null || text.Equals(""))
                {
                    throw new FormatException("Values for X not given!");
                }
                else
                {
                    if (Regex.IsMatch(text, xRegex))
                    {
                        Regex r = new Regex(" ");
                        string[] values = r.Split(text);

                        if (values.Length != dim)
                        {
                            throw new FormatException("Dimension of X: " + values.Length + " not matches dimension: " + dim + "!");
                        }
                        else
                        {
                            Point x = new Point(dim);
                            for (int i = 0; i < values.Length; i++)
                            {
                                x.p[i] = double.Parse(values[i]);
                            }
                            return x;
                        }
                    }
                    else
                    {
                        throw new FormatException("Values for X not valid!");
                    }
                }
            }
            else
            {
                return null;
            }
        }

        private void isRandomX_Checked(object sender, RoutedEventArgs e)
        {
            generateX = true;
            XValuesLabel.IsEnabled = false;
            XValuesTextBox.IsEnabled = false;
        }

        private void isRandomX_Unchecked(object sender, RoutedEventArgs e)
        {
            generateX = false;
            XValuesLabel.IsEnabled = true;
            XValuesTextBox.IsEnabled = true;
        }

        public void SampleChanged(object sender, EventArgs e)
        {
            this.sampleListView.ItemsSource = sampleGenerator.SampleArray;
        }

        private void savePointsButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<PointWrapper> points = (ObservableCollection<PointWrapper>)this.sampleListView.ItemsSource;
            if (points == null)
            {
                MainWindow.showMessage("No data available!");
            }
            else
            {
                FileManager manager = new FileManager();
                manager.trySavePoints(points);
            }
        }

        private void setAnnealing_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow annealing = new SettingsWindow();
            bool? result = annealing.ShowDialog();
            if (result == true)
            {
                Console.WriteLine(annealing.Settings.ToString());
                this.sett = annealing.Settings;
            }
        }
    }
}
