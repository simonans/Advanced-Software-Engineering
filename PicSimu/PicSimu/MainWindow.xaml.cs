using Gu.Wpf.DataGrid2D;
using Microsoft.Win32;
using PicSimu.Model;
using PicSimu.ViewModel;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PicSimu
{
    public partial class MainWindow : Window
    {
        private PIC pic;
        private PIC reset_pic;
        private MainViewModel viewModel;
        private bool isRun;

        //string filePath = "D:\\Git\\source\\repos\\PIC-Simulator\\TestFiles\\TPicSim12.LST";
        string filePath = "";
        //string filePath = "C:\\Users\\siman\\source\\repos\\PIC-Simulator\\TestFiles\\TPicSim12.LST";

        public MainWindow()
        {

            InitializeComponent();        
            
            if(File.Exists(filePath)) 
            {
                pic = new PIC(filePath);

                isRun = false;

                viewModel = new MainViewModel(pic);

                DataContext = viewModel;

                lstFileHighlighting();
            }
            else 
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "LST-Dateien (*.lst)|*.lst";
                openFileDialog.FilterIndex = 1;
                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    filePath = openFileDialog.FileName;

                    pic = new PIC(filePath);
                    isRun = false;
                    viewModel = new MainViewModel(pic);
                    DataContext = viewModel;
                    lstFileHighlighting();

                }
            }
        }

        public void reloadView(MainWindow mainWindow)
        {
            MainWindow neuesMainWindow = new MainWindow();
            mainWindow.Close();
            neuesMainWindow.Show();

            
        }

        private void Datei_Laden(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LST-Dateien (*.lst)|*.lst";
            openFileDialog.FilterIndex = 1;
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                filePath = openFileDialog.FileName;

                pic = new PIC(filePath);
                isRun = false;
                viewModel = new MainViewModel(pic);
                DataContext = viewModel;
                lstFileHighlighting();

            }

            //MainWindow currentMainWindow = Application.Current.MainWindow as MainWindow;
            //reloadView(currentMainWindow);
        }

        #region Datenspeicher
        //Wenn im Datenspeicher etwas geändert wird, wird das über diese Funktion an das Backend übertragen, da ein DataGrid2D kein TwoWay Binding kann
        private void Datenspeicher_CellEditEndiing(object obj, DataGridCellEditEndingEventArgs e)
        {
            var edetingBox = e.EditingElement as TextBox;
            string newValue = edetingBox.Text;
            int intValue = Convert.ToInt32(newValue, 16);

            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (IsHexadecimal(newValue) && intValue < 256)
                {
                    int row = e.Row.GetIndex();
                    int column = e.Column.DisplayIndex;
                    int index = row * 16 + column;
                    pic.datenspeicher.fromViewModel(newValue, index);
                    // viewModel.DATENSPEICHERVIEWMODEL;
                }
                else
                {
                    MessageBox.Show("Ungültige Hexadezimalzahl!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    viewModel = new MainViewModel(pic);
                    DataContext = viewModel;
                }

            }
        }

        private bool IsHexadecimal(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        private void Reload_Datareg(object sender, RoutedEventArgs e)
        {
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void Reset_Datareg(object sender, RoutedEventArgs e)
        {
            reset_pic = new PIC();
            pic.datenspeicher = reset_pic.datenspeicher;
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        #endregion



        #region Hilfsfktn für Tris und Port
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                if (checkBox.Content.ToString() == "i")
                    checkBox.Content = "o";
                else
                    checkBox.Content = "i";
            }
        }
        #endregion


        #region TrisA
        bool trisA0 = false;
        bool trisA1 = false;
        bool trisA2 = false;
        bool trisA3 = false;
        bool trisA4 = false;

        private void CheckBox_Click_TrisA_0(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[5] &= 0b_11111110;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[5] |= 0b_00000001;  //Input = Setzen
                trisA0 = !trisA0;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_TrisA_1(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[5] &= 0b_11111101;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[5] |= 0b_00000010;  //Input = Setzen
                trisA1 = !trisA1;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        private void CheckBox_Click_TrisA_2(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[5] &= 0b_11111011;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[5] |= 0b_00000100;  //Input = Setzen
                trisA2 = !trisA2;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_TrisA_3(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[5] &= 0b_11110111;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[5] |= 0b_00001000;  //Input = Setzen
                trisA3 = !trisA3;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_TrisA_4(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[5] &= 0b_11101111;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[5] |= 0b_00010001;  //Input = Setzen
                trisA4 = !trisA4;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        #endregion



        //Hier handle ich noch nicht was hinten dran passiert
        #region TRISB
        bool trisB0 = false;
        bool trisB1 = false;
        bool trisB2 = false;
        bool trisB3 = false;
        bool trisB4 = false;
        bool trisB5 = false;
        bool trisB6 = false;
        bool trisB7 = false;



        private void CheckBox_Click_TrisB_0(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[6] &= 0b_11111110;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[6] |= 0b_00000001;  //Input = Setzen
                trisB0 = !trisB0;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_TrisB_1(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[6] &= 0b_11111101;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[6] |= 0b_00000010;  //Input = Setzen
                trisB1 = !trisB1;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        private void CheckBox_Click_TrisB_2(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[6] &= 0b_11111011;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[6] |= 0b_00000100;  //Input = Setzen
                trisB2 = !trisB2;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_TrisB_3(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[6] &= 0b_11110111;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[6] |= 0b_00001000;  //Input = Setzen
                trisB3 = !trisB3;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_TrisB_4(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[6] &= 0b_11101111;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[6] |= 0b_00010001;  //Input = Setzen
                trisB4 = !trisB4;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_TrisB_5(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[6] &= 0b_11011110;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[6] |= 0b_00100000;  //Input = Setzen
                trisB5 = !trisB5;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_TrisB_6(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[6] &= 0b_10111111;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[6] |= 0b_01000000;  //Input = Setzen
                trisB6 = !trisB6;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_TrisB_7(object sender, RoutedEventArgs e)
        {
            CheckBox_Click(sender, e);
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.bank1[6] &= 0b_01111111;  //Output = Rücksetzen
                else
                    pic.datenspeicher.bank1[6] |= 0b_10000000;  //Input = Setzen
                trisB7 = !trisB7;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        #endregion



        #region PORTA
        private void CheckBox_Click_PortA_0(object sender, RoutedEventArgs e)
        {
            if (trisA0)  //TrisA0 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[5] |= 0b_00000001;
                else
                    pic.datenspeicher.storage[5] &= 0b_11111110;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_PortA_1(object sender, RoutedEventArgs e)
        {
            if (trisA1)  //TrisA1 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[5] |= 0b_00000010;
                else
                    pic.datenspeicher.storage[5] &= 0b_11111101;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        private void CheckBox_Click_PortA_2(object sender, RoutedEventArgs e)
        {
            if (trisA2)  //TrisA2 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[5] |= 0b_00000100;
                else
                    pic.datenspeicher.storage[5] &= 0b_11111011;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        private void CheckBox_Click_PortA_3(object sender, RoutedEventArgs e)
        {
            if (trisA3)  //TrisA3 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[5] |= 0b_00001000;
                else
                    pic.datenspeicher.storage[5] &= 0b_11110111;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        private void CheckBox_Click_PortA_4(object sender, RoutedEventArgs e)
        {
            if (trisA4)  //TrisA4 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[5] |= 0b_00010000;
                else
                    pic.datenspeicher.storage[5] &= 0b_11101111;
                pic.datenspeicher.ra4Input(isChecked);    //RBA TMR0 Handler
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        #endregion



        //Hier schreiben wir jeweils direkt in den Storage (also das PortRegister)
        #region PORTB

        private void CheckBox_Click_PortB_0(object sender, RoutedEventArgs e)
        {
            if (trisB0)  //TrisB0 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[6] |= 0b_00000001;
                else
                    pic.datenspeicher.storage[6] &= 0b_11111110;
                pic.datenspeicher.rb0Input(isChecked);  //RB0 Interrupt
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_PortB_1(object sender, RoutedEventArgs e)
        {
            if (trisB1)  //TrisB1 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[6] |= 0b_00000010;
                else
                    pic.datenspeicher.storage[6] &= 0b_11111101;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        private void CheckBox_Click_PortB_2(object sender, RoutedEventArgs e)
        {
            if (trisB2)  //TrisB2 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[6] |= 0b_00000100;
                else
                    pic.datenspeicher.storage[6] &= 0b_11111011;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        private void CheckBox_Click_PortB_3(object sender, RoutedEventArgs e)
        {
            if (trisB3)  //TrisB3 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[6] |= 0b_00001000;
                else
                    pic.datenspeicher.storage[6] &= 0b_11110111;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        private void CheckBox_Click_PortB_4(object sender, RoutedEventArgs e)
        {
            if (trisB4)  //TrisB4 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[6] |= 0b_00010000;
                else
                    pic.datenspeicher.storage[6] &= 0b_11101111;
                pic.datenspeicher.rb4to7Input();    //RB4 Interrupt
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_PortB_5(object sender, RoutedEventArgs e)
        {
            if (trisB5)  //TrisB5 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[6] |= 0b_00100000;
                else
                    pic.datenspeicher.storage[6] &= 0b_11011111;
                pic.datenspeicher.rb4to7Input();    //RB5 Interrupt
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_PortB_6(object sender, RoutedEventArgs e)
        {
            if (trisB6)  //TrisB6 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[6] |= 0b_01000000;
                else
                    pic.datenspeicher.storage[6] &= 0b_10111111;
                pic.datenspeicher.rb4to7Input();    //RB6 Interrupt
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void CheckBox_Click_PortB_7(object sender, RoutedEventArgs e)
        {
            if (trisB7)  //TrisB7 auf Ausgang gesetzt
                return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false;
                if (isChecked)
                    pic.datenspeicher.storage[6] |= 0b_10000000;
                else
                    pic.datenspeicher.storage[6] &= 0b_01111111;
                pic.datenspeicher.rb4to7Input();    //RB7 Interrupt
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        #endregion



        #region Watchdog
        private void WDT_enabled(object sender, RoutedEventArgs e)
        {
            pic.datenspeicher.enableWatchdog();
        }

        private void WDT_disabled(object sender, RoutedEventArgs e)
        {
            pic.datenspeicher.disableWatchdog();
        }
        #endregion


        #region Run
        private void Step_Click(object sender, RoutedEventArgs e)
        {
            oneStep();
        }

        
        private void Run_Click(object sender, RoutedEventArgs e)
        {
            isRun = !isRun;
            loop();
                
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            isRun = false;
            reset_pic = new PIC(filePath);
            pic = reset_pic;
            lstFileHighlighting();
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void oneStep()
        {
            pic.commandCycle();

            if (pic.datenspeicher.getWatchdogStatus())  //Watchdog Überlauf
                reset();

            if (pic.datenspeicher.getSleepStatus()) //Für Sleep Befehl
                standby();

            lstFileHighlighting();
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;

        }

        private void loop()
        {
            while (isRun)
            {
                oneStep();
                try
                {
                    Application.Current.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Background);
                }
                catch { isRun = false; }
            }  
        }

        private void standby()
        {
            while(!pic.datenspeicher.getWatchdogStatus())
            {
                pic.datenspeicher.watchdogHandler();
                Application.Current.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Background);
                viewModel = new MainViewModel(pic);
                DataContext = viewModel;
            }
            pic.datenspeicher.resetWatchdog();
        }

        private void reset()    //Nach Watchdog berlauf
        {
            pic.datenspeicher.datenspeicherReset();
            pic.w_Register.wregisterReset();
        }
        #endregion


        #region Highlighting und Breakpoints

        private int nextIndex;
        private int oldIndex;
        public void lstFileHighlighting()
        {
            //HighlightListBoxItemYellow(nextIndex);
            UnhighlightListBoxItem(oldIndex);


            int pc = pic.datenspeicher.getProgramCounter();

            LstFileLine currentline = viewModel.LSTFileLinePerLine.FirstOrDefault(x =>
            {
                string hexSubstring = x.Line.Substring(0, 4);
                int parsedValue;
                bool success = int.TryParse(hexSubstring, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out parsedValue);
                return success && parsedValue == pc;
            });

            if (currentline != null)
            {
                nextIndex = viewModel.LSTFileLinePerLine.IndexOf(currentline);
                oldIndex = nextIndex;
            }
            else
            {
                // Behandlung für den Fall, dass kein passendes Element gefunden wurde
            }

            listBox.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;

            if(currentline.Breakpoint == true)
            {
                MessageBox.Show("Halt Stop!!!");
                isRun = false;
            }

        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (listBox.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                HighlightListBoxItemYellow(nextIndex);
            }
        }

        private void HighlightListBoxItemYellow(int index)
        {
            if (index >= 0 && index < listBox.Items.Count)
            {
                ListBoxItem listBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
                if (listBoxItem != null)
                {
                    listBoxItem.Background = Brushes.Yellow;
                }
            }
        }

        private void UnhighlightListBoxItem(int index)
        {
            if (index >= 0 && index < listBox.Items.Count)
            {
                ListBoxItem listBoxItem = listBox.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
                if (listBoxItem != null)
                {
                    // Setze die Hintergrundfarbe des ListBoxItems zurück
                    listBoxItem.Background = Brushes.Transparent;
                }
            }
        }
        #endregion


        #region SFR

        #region STATUS

        private void Reset_Status_Reg(object sender, RoutedEventArgs e)
        {
            pic.datenspeicher.storage[3] = 24;
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }
        private void Irp_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (Irp_Flag_Textbox.Text == "0")
            {
                pic.datenspeicher.storage[3] += 128;
            }
            else
            {
                pic.datenspeicher.storage[3] -= 128;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void RP1_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (RP1_Flag_Textbox.Text == "0")
            {
                pic.datenspeicher.storage[3] += 64;
            }
            else
            {
                pic.datenspeicher.storage[3] -= 64;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void RP0_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (RP0_Flag_Textbox.Text == "0")
            {
                pic.datenspeicher.storage[3] += 32;
            }
            else
            {
                pic.datenspeicher.storage[3] -= 32;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void T0_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (TO_Flag_Textbox.Text == "0")
            {
                pic.datenspeicher.storage[3] += 16;
            }
            else
            {
                pic.datenspeicher.storage[3] -= 16;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void PD_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (PD_Flag_Textbox.Text == "0")
            {
                pic.datenspeicher.storage[3] += 8;
            }
            else
            {
                pic.datenspeicher.storage[3] -= 8;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void Zero_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (Zero_Flag_Textbox.Text == "0")
            {
                pic.datenspeicher.storage[3] += 4;
            }
            else
            {
                pic.datenspeicher.storage[3] -= 4;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void DC_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (DC_Flag_Textbox.Text == "0")
            {
                pic.datenspeicher.storage[3] += 2;
            }
            else
            {
                pic.datenspeicher.storage[3] -= 2;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void Carry_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (Carry_Flag_Textbox.Text == "0")
            {
                pic.datenspeicher.storage[3] += 1;
            }
            else
            {
                pic.datenspeicher.storage[3] -= 1;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        #endregion

        #region INTCON
        private void Reset_Intcon_Reg(object sender, RoutedEventArgs e)
        {
            pic.datenspeicher.storage[11] = 0;
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void GIE_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;

            if (GIE_Bit.Text == "0")
            {
                pic.datenspeicher.storage[11] += 128;
            }
            else
            {
                pic.datenspeicher.storage[11] -= 128;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void PIE_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;

            if (PIE_Bit.Text == "0")
            {
                pic.datenspeicher.storage[11] += 64;
            }
            else
            {
                pic.datenspeicher.storage[11] -= 64;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void T0IE_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;

            if (T0IE_Bit.Text == "0")
            {
                pic.datenspeicher.storage[11] += 32;
            }
            else
            {
                pic.datenspeicher.storage[11] -= 32;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void INTE_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;

            if (INTE_Bit.Text == "0")
            {
                pic.datenspeicher.storage[11] += 16;
            }
            else
            {
                pic.datenspeicher.storage[11] -= 16;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void RBIE_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;

            if (RBIE_Bit.Text == "0")
            {
                pic.datenspeicher.storage[11] += 8;
            }
            else
            {
                pic.datenspeicher.storage[11] -= 8;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void T0IF_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;

            if (T0IF_Bit.Text == "0")
            {
                pic.datenspeicher.storage[11] += 4;
            }
            else
            {
                pic.datenspeicher.storage[11] -= 4;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void INTF_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;

            if (INTF_Bit.Text == "0")
            {
                pic.datenspeicher.storage[11] += 2;
            }
            else
            {
                pic.datenspeicher.storage[11] -= 2;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void RBIF_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;

            if (RBIF_Bit.Text == "0")
            {
                pic.datenspeicher.storage[11] += 1;
            }
            else
            {
                pic.datenspeicher.storage[11] -= 1;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        #endregion

        #region OPTION

        private void Reset_Option_Reg(object sender, RoutedEventArgs e)
        {
            pic.datenspeicher.bank1[1] = 255;
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void RPBU_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (RBPU_Bit.Text == "0")
            {
                pic.datenspeicher.bank1[1] += 128;
            }
            else
            {
                pic.datenspeicher.bank1[1] -= 128;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void INTEDG_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (INTEDG_Bit.Text == "0")
            {
                pic.datenspeicher.bank1[1] += 64;
            }
            else
            {
                pic.datenspeicher.bank1[1] -= 64;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void T0CS_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (T0CS_Bit.Text == "0")
            {
                pic.datenspeicher.bank1[1] += 32;
            }
            else
            {
                pic.datenspeicher.bank1[1] -= 32;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void T0SE_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (T0SE_Bit.Text == "0")
            {
                pic.datenspeicher.bank1[1] += 16;
            }
            else
            {
                pic.datenspeicher.bank1[1] -= 16;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void PSA_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (PSA_Bit.Text == "0")
            {
                pic.datenspeicher.bank1[1] += 8;
            }
            else
            {
                pic.datenspeicher.bank1[1] -= 8;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void PS2_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (PS2_Bit.Text == "0")
            {
                pic.datenspeicher.bank1[1] += 4;
            }
            else
            {
                pic.datenspeicher.bank1[1] -= 4;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void PS1_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (PS1_Bit.Text == "0")
            {
                pic.datenspeicher.bank1[1] += 2;
            }
            else
            {
                pic.datenspeicher.bank1[1] -= 2;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void PS0_Handler(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (PS0_Bit.Text == "0")
            {
                pic.datenspeicher.bank1[1] += 1;
            }
            else
            {
                pic.datenspeicher.bank1[1] -= 1;
            }
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        #endregion


        private void Reset_Pcl_Reg(object sender, RoutedEventArgs e)
        {
            pic.datenspeicher.storage[2] = 0;
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void Reset_Pclath_Reg(object sender, RoutedEventArgs e)
        {
            pic.datenspeicher.storage[10] = 0;
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void Reset_W_Reg(object sender, RoutedEventArgs e)
        {
            pic.w_Register.writeIn(0);
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void Reset_Fsr_Reg(object sender, RoutedEventArgs e)
        {
            pic.datenspeicher.storage[4] = 0;
            viewModel = new MainViewModel(pic);
            DataContext = viewModel;
        }

        private void Reset_All_SFReg(object sender, RoutedEventArgs e)
        {
            Reset_Fsr_Reg(sender, e);
            Reset_W_Reg(sender, e);
            Reset_Pcl_Reg(sender, e);
            Reset_Pclath_Reg(sender, e);
            Reset_Status_Reg(sender, e);
            Reset_Intcon_Reg(sender, e);
            Reset_Option_Reg(sender, e);
        }


        #endregion

    }
}