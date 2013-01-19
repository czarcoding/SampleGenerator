using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace SampleGen
{
    public delegate void SampleChangedEventHandler(object sender, EventArgs e);

    class SampleGenerator
    {
        private RandomPointGenerator generator;
        private PlaneProjector planeProjector;
        private PointProcessor pointProcessor;
        private ObservableCollection<PointWrapper> sampleArray;

        private BackgroundWorker bgWorker;
        private ProgressWindow progressWindow;


        public event SampleChangedEventHandler SampleArrayChanged;

        public ObservableCollection<PointWrapper> SampleArray
        {
            get { return sampleArray; }
            set
            {
                sampleArray = value;
                OnChanged(EventArgs.Empty);
            }
        }
        protected virtual void OnChanged(EventArgs e)
        {
            if (SampleArrayChanged != null)
                SampleArrayChanged(this, e);
        }

        public bool run(int n, int dimension, double c, Point X)
        {
            generator = new RandomPointGenerator();
            planeProjector = new PlaneProjector();
            pointProcessor = new PointProcessor();

            Console.WriteLine("=================== NEW RUN ===================\n");

            if (X == null)
            {
                X = generator.generatePoint(dimension);
            }

            Console.WriteLine("X= " + X.ToString() + "\n");

            //Preparing background worker;
            prepareObjects();

            Object[] arguments = { n, dimension, c, X };

            bgWorker.RunWorkerAsync(arguments);

            return true;
        }

        private void backgroundWorker_DoWork(object s, DoWorkEventArgs args)
        {
            Object[] arguments = (Object[])args.Argument;
            int n = (int)arguments[0];
            int dim = (int)arguments[1];
            double d = (double)arguments[2];
            Point X = (Point)arguments[3];

            ObservableCollection<PointWrapper> sample = new ObservableCollection<PointWrapper>();
            int totalPoints = 0;
            int validPoint = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            //MAIN PROCEDURE LOOP!!
            while (sample.Count < n)
            {

                if (bgWorker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }

                Point point = null;

                Console.WriteLine("\nPOINT No " + (totalPoints + 1) + " \n");
                point = planeProjector.makePoint(X, d, dim);

                point = pointProcessor.processPoint(point, X);

                PointWrapper pointData = new PointWrapper(point, X, d);
                Console.WriteLine("Sum is: "+pointData.Sum);
                pointData.calculateMean(X);
                Console.WriteLine("Mean is: " + pointData.Mean);
                if (pointData.IsValid)
                {
                    sample.Add(pointData);
                    validPoint++;
                }
                totalPoints++;

                double time = stopwatch.ElapsedMilliseconds;
                UpdateProgressDelegate update = new UpdateProgressDelegate(UpdateProgressText);
                progressWindow.Dispatcher.BeginInvoke(update, (validPoint * 100) / n, totalPoints, time);
            }
            stopwatch.Stop();

            args.Result = sample;
        }
        private void backgroundWorker_RunWorkerCompleted(object s, RunWorkerCompletedEventArgs args)
        {
            if (!args.Cancelled)
            {
                this.SampleArray = (ObservableCollection<PointWrapper>)args.Result;
            }
        }

        public void cancelWork(object sender, EventArgs e)
        {
            if (bgWorker != null)
            {
                bgWorker.CancelAsync();
                progressWindow.Close();
            }
        }

        public delegate void UpdateProgressDelegate(int percentage, int totalPoints, double time);
        public void UpdateProgressText(int percentage, int totalPoints, double time)
        {
            TimeSpan span = TimeSpan.FromMilliseconds(time);

            //set our progress dialog text and value
            progressWindow.ProgressText = string.Format("Progress: {0}%", percentage.ToString());
            progressWindow.ProgressValue = percentage;
            progressWindow.TimeText = string.Format("Computation time: {0}\nTotal generated: {1} points", formatTimeSpan(span), totalPoints);
        }
        private string formatTimeSpan(TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
              span.Duration().Days > 0 ? string.Format("{0:0} d, ", span.Days) : string.Empty,
              span.Duration().Hours > 0 ? string.Format("{0:0} h, ", span.Hours) : string.Empty,
              span.Duration().Minutes > 0 ? string.Format("{0:0} m, ", span.Minutes) : string.Empty,
              span.Duration().Seconds > 0 ? string.Format("{0:0} s", span.Seconds) : string.Empty,
              span.Duration().Milliseconds > 0 ? string.Format("{0:0} ms", span.Milliseconds) : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 s";

            return formatted;
        }
        private void prepareObjects()
        {
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;

            bgWorker.DoWork += backgroundWorker_DoWork;
            bgWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            progressWindow = new ProgressWindow();
            progressWindow.Cancel += cancelWork;

            progressWindow.Show();
        }

    }
}
