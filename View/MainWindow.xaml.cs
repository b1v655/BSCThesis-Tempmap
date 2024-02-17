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

namespace WpfApp2.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model.RBFmodel _model;
        public MainWindow(Model.RBFmodel model)
        {
            _model = model;
            InitializeComponent();
            
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            StatusBarText.Text="X: " +(int)Mouse.GetPosition(ParentCv).X + " Y: " + (int)Mouse.GetPosition(ParentCv).Y + " Érték: " + _model.Value((int)Mouse.GetPosition(ParentCv).Y, (int)Mouse.GetPosition(ParentCv).X)+" °C";
            
        }
        private void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                _model.PointIncrease();

            else if (e.Delta < 0)
                _model.PointDecrease();
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            ParentCv.Width = ActualWidth;
            ParentCv.Height = ActualHeight;
            Image image;
            image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);
            ParentCv.Children.Clear();
            image.Source = _model.GetMap((int)ParentCv.Width, (int)ParentCv.Height);
            ParentCv.Children.Add(image);
            image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }
    }
}
