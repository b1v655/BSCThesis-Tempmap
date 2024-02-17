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
    class RBFviewModel
    {


        private Model.RBFmodel _model;
        private View.MainWindow _view;
        
        public DelegateCommand CholeskyCommand { get; private set; }
        public DelegateCommand GECommand { get; private set; }
        public DelegateCommand GESPPCommand { get; private set; }
        public DelegateCommand MQCommand { get; private set; }
        public DelegateCommand IQCommand { get; private set; }
        public DelegateCommand GCommand { get; private set; }
        public DelegateCommand IMQCommand { get; private set; }
        public DelegateCommand TPSCommand { get; private set; }
        public DelegateCommand PointPasteCommand { get; private set; }
        public DelegateCommand PointRemoveCommand { get; private set; }
        public DelegateCommand SetFirstColorCommand { get; private set; }
        public DelegateCommand SetSecondColorCommand { get; private set; }
        public DelegateCommand SetThirdColorCommand { get; private set; }
        public DelegateCommand SetForthColorCommand { get; private set; }
        public DelegateCommand SetFifthColorCommand { get; private set; }
        public DelegateCommand SetSixthColorCommand { get; private set; }
        public DelegateCommand SetAllColorCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand SaveImageCommand { get; private set; }
        public DelegateCommand SavePointsCommand { get; private set; }
        public DelegateCommand LoadPointsCommand { get; private set; }
        public DelegateCommand CABCommand { get; private set; }
        public DelegateCommand NCABCommand { get; private set; }

        public DelegateCommand NewMapCommand { get; private set; }

        public DelegateCommand ChoosedPointCommand { get; private set; }
        public DelegateCommand EpsylonFiveCommand { get; private set; }
        public DelegateCommand EpsylonOneCommand { get; private set; }
        public DelegateCommand EpsylonNullFiveCommand { get; private set; }
        public DelegateCommand EpsylonNullOneCommand { get; private set; }
        public DelegateCommand ThreeDViewCommand { get; private set; }
        public RBFviewModel(Model.RBFmodel model, View.MainWindow view)
        {
            _model = model;
            _view = view;


            CholeskyCommand = new DelegateCommand(param => ChangeFT(Model.LESCalculate.CD));
            GECommand = new DelegateCommand(param => ChangeFT(Model.LESCalculate.GE));
            GESPPCommand = new DelegateCommand(param => ChangeFT(Model.LESCalculate.GESPP));

            MQCommand = new DelegateCommand(param => ChangeType(Model.psiFunctionType.MultiQuadric));
            IQCommand = new DelegateCommand(param => ChangeType(Model.psiFunctionType.InverseQuadratic));
            GCommand = new DelegateCommand(param => ChangeType(Model.psiFunctionType.Gaußian));
            IMQCommand = new DelegateCommand(param => ChangeType(Model.psiFunctionType.InverseMultiQuadric));
            TPSCommand = new DelegateCommand(param => ChangeType(Model.psiFunctionType.ThinPlateSpline));

            CABCommand = new DelegateCommand(param => CAB(true));
            NCABCommand = new DelegateCommand(param => CAB(false));

            PointPasteCommand = new DelegateCommand(param => PointPaste());
            PointRemoveCommand = new DelegateCommand(param => PointRemove());

            SetFirstColorCommand = new DelegateCommand(param => SetFirstColor());
            SetSecondColorCommand = new DelegateCommand(param => SetSecondColor());
            SetThirdColorCommand = new DelegateCommand(param => SetThirdColor());
            SetForthColorCommand = new DelegateCommand(param => SetForthColor());
            SetFifthColorCommand = new DelegateCommand(param => SetFifthColor());
            SetSixthColorCommand = new DelegateCommand(param => SetSixthColor());
            SetAllColorCommand = new DelegateCommand(param => SetAllColor());
            RefreshCommand = new DelegateCommand(param => RefreshMap());
            NewMapCommand = new DelegateCommand(param => askBox());
            ThreeDViewCommand = new DelegateCommand(param => new ThreeDVM(_model,new View.ThreeDView(), _view.ActualWidth, _view.ActualHeight));
            SaveImageCommand = new DelegateCommand(param => SaveImage());
            SavePointsCommand = new DelegateCommand(param => SaveData());
            LoadPointsCommand = new DelegateCommand(param => LoadData());
            ChoosedPointCommand = new DelegateCommand(param => SelectedPoint());
            EpsylonFiveCommand = new DelegateCommand(param => { _model.epsylon = 0.005; RefreshMap();});
            EpsylonOneCommand = new DelegateCommand(param => {_model.epsylon = 0.001; RefreshMap(); });
            EpsylonNullFiveCommand = new DelegateCommand(param => {_model.epsylon = 0.0005; RefreshMap(); });
            EpsylonNullOneCommand = new DelegateCommand(param => {_model.epsylon = 0.0001; RefreshMap(); });

        }
        private void SelectedPoint()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            _view.ParentCv.Width = _view.ActualWidth;
            _view.ParentCv.Height = _view.ActualHeight;
            Image image;
            image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);
            _view.ParentCv.Children.Clear();
            image.Source = _model.SelectPoint((int)_view.ParentCv.Width, (int)_view.ParentCv.Height, (int)System.Windows.Input.Mouse.GetPosition(_view.ParentCv).Y, (int)System.Windows.Input.Mouse.GetPosition(_view.ParentCv).X);
            _view.ParentCv.Children.Add(image);
            image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            
        }
        private void askBox()
        {
            MessageBoxResult result = MessageBox.Show("Menti mielőtt új térképhez kezdene?", "Új térkép", MessageBoxButton.YesNoCancel);

            switch(result)
            {
                case MessageBoxResult.Yes:
                    SaveData();
                    _view.ParentCv.Children.Clear();
                    _model.NewTable();
                    break;
                case MessageBoxResult.No:
                    _view.ParentCv.Children.Clear();
                    _model.NewTable();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
        private void ChangeFT(Model.LESCalculate ft)
        {
            _model.ChangeFT(ft);
            RefreshMap();
        }
        private void ChangeType(Model.psiFunctionType ft)
        {
            _model.ChangeRBFType(ft);
            RefreshMap();
        }
        private void RefreshMap()
        {
            
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            _view.ParentCv.Width = _view.ActualWidth;
            _view.ParentCv.Height = _view.ActualHeight;
            Image image;
            image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);
            _view.ParentCv.Children.Clear();
            image.Source = _model.GetMap((int)_view.ParentCv.Width,(int)_view.ParentCv.Height);
            _view.ParentCv.Children.Add(image);
            image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }
       private void PointPaste()
        {
            
            double x = System.Windows.Input.Mouse.GetPosition(_view.ParentCv).X;
            double y = System.Windows.Input.Mouse.GetPosition(_view.ParentCv).Y;
            _model.AddPoint(y, x,15);
            RefreshMap();
        }
        private void PointRemove()
        {
            if(_model.RemovePoint())
                RefreshMap();
        }
        
        private void SetAllColor()
        {
            _model.SetFirstColor();
            _model.SetSecondColor();
            _model.SetThirdColor();
            _model.SetForthColor();
            _model.SetFifthColor();
            _model.SetSixthColor();
            RefreshMap();

        }
        
        private void CAB(bool b)
        {
            _model.cab = b;
            RefreshMap();
        }

        private void SetFirstColor()
        {
            _model.SetFirstColor();
            RefreshMap();
        }
        private void SetSecondColor()
        {
            _model.SetSecondColor();
            RefreshMap();
        }
        private void SetThirdColor()
        {
            _model.SetThirdColor();
            RefreshMap();
        }
        private void SetForthColor()
        {
            _model.SetForthColor();
            RefreshMap();
        }
        private void SetFifthColor()
        {
            _model.SetFifthColor();
            RefreshMap();
        }
        private void SetSixthColor()
        {
            _model.SetSixthColor();
            RefreshMap();
        }
        private void SaveImage()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógusablak
            saveFileDialog.Title = "Kép mentése";
            saveFileDialog.Filter = "PNG File (*.png) | *.png";
            if (saveFileDialog.ShowDialog() == true)
            {
                _model.saveMap(_view.ParentCv, saveFileDialog.FileName);

            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }
        private async void SaveData()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógusablak
            saveFileDialog.Title = "Pontok mentése fájlba";
            saveFileDialog.Filter = "MapFile (*.mf) | *.mf";
            if (saveFileDialog.ShowDialog() == true)
            {
                await _model.SaveGame(saveFileDialog.FileName);

            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }
        private async void LoadData()
        {
            
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Pontok betöltése fájlból";
            openFileDialog.Filter = "MapFile (*.mf) | *.mf";
            if (openFileDialog.ShowDialog() == true)
                {
                    await _model.LoadGame(openFileDialog.FileName);

                }
            
            RefreshMap();
        }

    }
}
