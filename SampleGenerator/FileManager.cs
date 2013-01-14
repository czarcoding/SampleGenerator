using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;

namespace SampleGen
{
    class FileManager
    {
        FileDialog fileDialog;

        public void trySavePoints(ObservableCollection<PointWrapper> points)
        {
            if (fileDialog == null)
            {
                fileDialog = new SaveFileDialog();
            }

            string fileName = getFileName();
            
            if (fileName != null)
            {
                savePoints(fileName, points);
            }
        }
        public string getFileName()
        {
            fileDialog.DefaultExt = ".txt";
            fileDialog.Filter = "Text documents (.txt)|*.txt";

            if (fileDialog.ShowDialog() == true)
            {
                return fileDialog.FileName;
            }
            else
            {
                return null;
            }
        }
        public void savePoints(string path, ObservableCollection<PointWrapper> points)
        {
            using (StreamWriter file = new StreamWriter(@path))
            {

                foreach (PointWrapper pw in points)
                {
                    file.WriteLine(pw.Point.ToString());
                }
            }
        }

    }
}
