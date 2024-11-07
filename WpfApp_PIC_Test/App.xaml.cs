using System.Windows;
using WpfApp_PIC.Anwednungsschicht;
using WpfApp_PIC.Domänenschicht;
using WpfApp_PIC.Adapterschicht.ViewModel;
using WpfApp_PIC.Pluginschicht.View;
using System.ComponentModel;

namespace WpfApp_PIC
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
         
            base.OnStartup(e);

            string filePath = "lalelu";

            // Initialisierung
            var pic = new PIC(filePath);
            var dataRegister = new DataRegister();
            var dataRegisterService = new DataRegisterService(dataRegister);
            var viewModel = new DataRegisterViewModel(dataRegisterService);


            var mainWindow = new MainWindow
            {
                DataContext = viewModel
            };
            mainWindow.ShowDialog();
        }
    }
}
