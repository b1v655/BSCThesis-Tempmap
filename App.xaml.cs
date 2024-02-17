using System;
using System.Windows;

namespace WpfApp2
{
   
 
    public partial class App : Application
    {
        private Model.RBFmodel _model;
        private ViewModel.RBFviewModel _viewModel;
        private View.MainWindow _view;
        

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {

            _model = new Model.RBFmodel(new Persistence.RBFPersistenceDataAccess());

            


            _view = new View.MainWindow(_model);

            _viewModel = new ViewModel.RBFviewModel(_model,_view);


            _view.DataContext = _viewModel;
            _view.Show();

        }
    }
}
