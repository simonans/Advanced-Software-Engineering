using System.Windows;
using WpfApp_PIC.Anwednungsschicht;
using WpfApp_PIC.Domänenschicht;
using WpfApp_PIC.Adapterschicht.ViewModel;
using WpfApp_PIC.Pluginschicht.View;
using System.ComponentModel;
using Microsoft.Win32;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;
using System.Net.WebSockets;
using WpfApp_PIC.Pluginschicht.LST_File_Reader;
using System.Reflection.PortableExecutable;
using WpfApp_PIC.Adapterschicht.Parser;

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


            //Pluginschicht initialisieren: Datei-Reader und Parser
            var reader = new LST_File_Reader();
            var parser = new Parser(reader);

            //Zentrale PIC-Instanz
            var pic = new PIC(filePath, parser);

            //Anwendungsschicht initialisieren: Services
            var stackService = new StackService(pic.GetStack());
            var wRegisterService = new W_RegisterService(pic.GetW_Register());
            var programMemoryService = new ProgramMemoryService(pic.GetProgramMemory());
            var programCounterService = new ProgrammCounterService(pic.GetProgramCounter());
            var dataRegisterService = new DataRegisterService(pic.GetDataRegister());
            var pclathRegisterService = new PCLATHRegisterService(pic.GetDataRegister());
            var pclRegisterService = new PCLRegisterService(pic.GetDataRegister());
            var statusRegisterService = new StatusRegisterService(pic.GetDataRegister());
            var tmr0RegisterService = new TMR0RegisterService(pic.GetDataRegister());

            // Adapterschicht initialisieren: ViewModel
            var dataRegisterViewModel = new DataRegisterViewModel(dataRegisterService, statusRegisterService, pclathRegisterService, pclRegisterService, tmr0RegisterService);

            /////////////////////////////////////////TODO///////////////////////////////////////////
            var mainViewModel = new MainViewModel(dataRegisterViewModel);
            /////////////////////////////////////////TODO///////////////////////////////////////////

            var mainWindow = new MainWindow
            {
                DataContext = dataRegisterViewModel
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
