using System.Windows;
using WpfApp_PIC.Anwednungsschicht;
using WpfApp_PIC.Domänenschicht;
using WpfApp_PIC.Adapterschicht.ViewModel;
using WpfApp_PIC.Pluginschicht.View;
using System.ComponentModel;
using Microsoft.Win32;

namespace WpfApp_PIC
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
         
            base.OnStartup(e);

            // Datei-Pfad abfragen
            string filePath = GetLstFilePath();
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Keine Datei ausgewählt. Die Anwendung wird aber nicht beendet weil ich das halt nicht will.");
                //Shutdown();
                //return;
            }

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

        private string? GetLstFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "LST-Dateien (*.lst)|*.lst",
                FilterIndex = 1
            };

            bool? result = openFileDialog.ShowDialog();
            return result == true ? openFileDialog.FileName : null;
        }
    }
}
