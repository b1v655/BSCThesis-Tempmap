using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Microsoft.Win32;
namespace WpfApp2.ViewModel
{
    class ThreeDVM
    {
        private Model.RBFmodel _model;
        private View.ThreeDView _view;
        private double _parentWidth;
        private double _parentHeight;
        public ThreeDVM(Model.RBFmodel model, View.ThreeDView view,double parentWidth,double parentHeight)
        {
            _parentHeight = parentHeight;
            _parentWidth = parentWidth;

            _model = model;
            _view = view;
            _view.Show();
            DrawFunction();
        }

        private void DrawFunction()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            _view.ParentCv.Width = _parentWidth;
            _view.ParentCv.Height = _parentHeight;
            Image image;
            image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);
            image.Source = _model.GetThreeDMap((int)_parentWidth, (int)_parentHeight,20);
            _view.ParentCv.Children.Add(image);
            image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }
    }
}
