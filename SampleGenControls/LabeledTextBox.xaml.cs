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

namespace SampleGenControls
{
    /// <summary>
    /// Interaction logic for LabeledTextBox.xaml
    /// </summary>
    public partial class LabeledTextBox : UserControl
    {
        public LabeledTextBox()
        {
            InitializeComponent();
        }
        public string LabelText
        {
            set
            {
                this.LabelComponent.Content = value;
            }
            get
            {
                return this.LabelComponent.Content.ToString();
            }
        }
        public KeyEventHandler TextBoxKeyDown
        {
            set{
                this.TextFieldComponent.KeyDown += value;
            }
        }

        public string TextBoxText
        {
            set
            {
                this.TextFieldComponent.Text = value;
            }
            get
            {
                return this.TextFieldComponent.Text;
            }
        }

        public double LabelWidth
        {
            set
            {
                this.LabelColumnWidth.Width = new GridLength(value);
            }
            get
            {
                return this.LabelColumnWidth.ActualWidth;
            }
        }
        public void setLabelAutoWidth()
        {
            this.LabelColumnWidth.Width = new GridLength(1, GridUnitType.Auto);
        }
        public void TextBoxFocus()
        {
            this.TextFieldComponent.Focus();
        }
    }
}
