using PicSimu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace PicSimu.ViewModel
{
    internal class MainViewModel : BindingMechanismBase
    {

        private Parser parserViewModel;
        private string[,] datenspeicherViewModel; 
        private ObservableCollection<LstFileLine> lstFileLinePerLine;


        public ObservableCollection<LstFileLine> LSTFileLinePerLine
        {
            get { return lstFileLinePerLine; }
            set
            {
                lstFileLinePerLine = value;
                OnPropertyChanged(nameof(LSTFileLinePerLine));
            }
        }

        public string[,] DATENSPEICHERVIEWMODEL
        {
            get { return datenspeicherViewModel; }
            //set wird nicht benötigt
            set
            {
                SetProperty(ref datenspeicherViewModel, value, nameof(DATENSPEICHERVIEWMODEL));
            }
        }

        private static int initializer;

        private PIC picOneWayBindingInstance;
        private W_Register w_Register;
        public MainViewModel(PIC pic)
        {

            
            picOneWayBindingInstance = pic;
            w_Register = pic.w_Register;

            parserViewModel = pic.parser;
            datenspeicherViewModel = pic.datenspeicher.toViewModel();
            LSTFileLinePerLine = new ObservableCollection<LstFileLine>(parserViewModel.LSTFileLinePerLine);
        }

        #region SFR


        #region Statusregister
        public string StatusReg
        {
            get { return picOneWayBindingInstance.datenspeicher.storage[3].ToString("X2"); }
            //set
            //{
            //    if (picOneWayBindingInstance.datenspeicher.storage[3].ToString("X") != value)
            //    {
            //        string tmp = picOneWayBindingInstance.datenspeicher.storage[3].ToString("X");
            //         tmp = value;
            //        OnPropertyChanged(nameof(StatusReg));
            //    }
            //}
        }

        public int CarryFlag
        {
            get { return picOneWayBindingInstance.datenspeicher.getBitFromRegister(3, 0); }
            //set
            //{
            //    if ((picOneWayBindingInstance.datenspeicher.getBitFromRegister(3, 0)) != value)
            //    {
            //        int tmp = (picOneWayBindingInstance.datenspeicher.getBitFromRegister(3, 0));
            //        tmp = value;
            //        OnPropertyChanged(nameof(CarryFlag));
            //    }
            //}
        }

        public int DigitalcarryFlag
        {
            get { return picOneWayBindingInstance.datenspeicher.getBitFromRegister(3, 1); }
        }

        public int ZeroFlag
        {
            get { return picOneWayBindingInstance.datenspeicher.getBitFromRegister(3, 2); }
        }

        public int PowerdownFlag
        {
            get { return picOneWayBindingInstance.datenspeicher.getBitFromRegister(3, 3); }
        }

        public int TimeoutFlag
        {
            get { return picOneWayBindingInstance.datenspeicher.getBitFromRegister(3, 4); }
        }

        public int Rp0Flag
        {
            get { return picOneWayBindingInstance.datenspeicher.getBitFromRegister(3, 5); }
        }

        public int Rp1Flag
        {
            get { return picOneWayBindingInstance.datenspeicher.getBitFromRegister(3, 6); }
        }

        public int IrpFlag
        {
            get { return picOneWayBindingInstance.datenspeicher.getBitFromRegister(3, 7); }
        }
        #endregion

        ///////////////////////////////////////////////////////

        public string PclReg
        {
            get { return picOneWayBindingInstance.datenspeicher.storage[2].ToString("X2"); }
        }

        public string PclathReg
        {
            get { return picOneWayBindingInstance.datenspeicher.storage[10].ToString("X2"); }
        }

        public string W_Reg
        {


            get {
                return w_Register.getValue().ToString("X2"); 
            }
        }

        public string FsrReg
        {
            get { return picOneWayBindingInstance.datenspeicher.storage[4].ToString("X2"); }
        }

        ///////////////////////////////////////////////////////

        #region OPTION
        public string OptionReg
        {
            get { return picOneWayBindingInstance.datenspeicher.bank1[1].ToString("X2"); }
        }

        public int PS0
        {
            
            get { return (picOneWayBindingInstance.datenspeicher.getBitFromOptionReg(1)); }
        }

        public int PS1
        {
            get { return (picOneWayBindingInstance.datenspeicher.getBitFromOptionReg(2)); }
        }

        public int PS2
        {
            get { return (picOneWayBindingInstance.datenspeicher.getBitFromOptionReg(4)); }
        }

        public int PSA
        {
            get { return (picOneWayBindingInstance.datenspeicher.getBitFromOptionReg(8)); }
        }

        public int T0SE
        {
            get { return (picOneWayBindingInstance.datenspeicher.getBitFromOptionReg(16)); }
        }

        public int T0CS
        {
            get { return (picOneWayBindingInstance.datenspeicher.getBitFromOptionReg(32)); }
        }


        public int IntEdg
        {
            get { return (picOneWayBindingInstance.datenspeicher.getBitFromOptionReg(64)); }
        }

        public int RBPU
        {
            get { return (picOneWayBindingInstance.datenspeicher.getBitFromOptionReg(128)); }
        }
        #endregion

        #region INTCON
        public string IntconReg
        {
            get { return picOneWayBindingInstance.datenspeicher.storage[11].ToString("X2"); }
        }

        public int RBIF
        {

            get { return (picOneWayBindingInstance.datenspeicher.getBitFromIntconReg(1)); }
        }

        public int INTF
        {

            get { return (picOneWayBindingInstance.datenspeicher.getBitFromIntconReg(2)); }
        }

        public int T0IF
        {

            get { return (picOneWayBindingInstance.datenspeicher.getBitFromIntconReg(4)); }
        }

        public int RBIE
        {

            get { return (picOneWayBindingInstance.datenspeicher.getBitFromIntconReg(8)); }
        }

        public int INTE
        {

            get { return (picOneWayBindingInstance.datenspeicher.getBitFromIntconReg(16)); }
        }

        public int T0IE
        {

            get { return (picOneWayBindingInstance.datenspeicher.getBitFromIntconReg(32)); }
        }

        public int PIE
        {

            get { return (picOneWayBindingInstance.datenspeicher.getBitFromIntconReg(64)); }
        }

        public int GIE
        {

            get { return (picOneWayBindingInstance.datenspeicher.getBitFromIntconReg(128)); }
        }
        #endregion

        public int Pc
        {
            get { return picOneWayBindingInstance.datenspeicher.getProgramCounter(); }

        }



        #endregion

        #region LED Arrays
        public bool IsLED0On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortBOut(0);
                //if (picOneWayBindingInstance.datenspeicher.getBitFromTrisBReg(1) == false)
                //{
                //    return picOneWayBindingInstance.datenspeicher.getBitFromPortRBReg(1);
                //}
                //else { return false; }
            }
            //set
            //{
            //    if (picOneWayBindingInstance.datenspeicher.getBitFromPortRBReg(1) != value)
            //    {
            //        bool tmp = picOneWayBindingInstance.datenspeicher.getBitFromPortRBReg(1);
            //        value = tmp;
            //        OnPropertyChanged(nameof(IsLED0On));
            //    }
            //}

        }

        public bool IsLED1On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortBOut(1);
            }
        }

        public bool IsLED2On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortBOut(2);
            }
        }

        public bool IsLED3On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortBOut(3);
            }
        }

        public bool IsLED4On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortBOut(4);
            }
        }

        public bool IsLED5On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortBOut(5);
            }
        }

        public bool IsLED6On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortBOut(6);
            }
        }

        public bool IsLED7On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortBOut(7);
            }
        }
       

        public bool IsLEDRA0On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortAOut(0);
            }
        }

        public bool IsLEDRA1On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortAOut(1);
            }
        }

        public bool IsLEDRA2On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortAOut(2);
            }
        }

        public bool IsLEDRA3On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortAOut(3);
            }
        }

        public bool IsLEDRA4On
        {
            get
            {
                return picOneWayBindingInstance.datenspeicher.latch.getBitInPortAOut(4);
            }
        }
        #endregion

        #region Laufzeitzähler und Frequenz
        private string _Laufzeit;
        public string Laufzeit
        {
            get { return picOneWayBindingInstance.datenspeicher.getDuration().ToString(); }
        }

        private ObservableCollection<double> _options;
        public ObservableCollection<double> Options
        {
            get { return _options; }
            set
            {
                _options = value;
                OnPropertyChanged(nameof(Options));
            }
        }


        public double Frequency
        {
            get { return picOneWayBindingInstance.datenspeicher.frequency; }
            set
            {
                if(picOneWayBindingInstance.datenspeicher.frequency != value)
                {
                    picOneWayBindingInstance.datenspeicher.frequency = value;
                    OnPropertyChanged(nameof(Frequency));
                }
                
            }
        }
        #endregion

        #region Stack
        public string stackPointer
        {
            get { return picOneWayBindingInstance.stack.stackPointer.ToString(); }
        }

        public string stackElement0
        {
            get { return picOneWayBindingInstance.stack.stackStorage[0].ToString(); }
            //set
            //{
            //    if (picOneWayBindingInstance.stack.stackStorage[0].ToString() != value)
            //    {
            //        string tmp = picOneWayBindingInstance.stack.stackStorage[0].ToString();
            //        tmp = value;
            //        OnPropertyChanged(nameof(stackElement0));
            //    }

            //}
        }

        public string stackElement1
        {
            get { return picOneWayBindingInstance.stack.stackStorage[1].ToString(); }
        }
        public string stackElement2
        {
            get { return picOneWayBindingInstance.stack.stackStorage[2].ToString(); }
        }

        public string stackElement3
        {
            get { return picOneWayBindingInstance.stack.stackStorage[3].ToString(); }
        }

        public string stackElement4
        {
            get { return picOneWayBindingInstance.stack.stackStorage[4].ToString(); }
        }

        public string stackElement5
        {
            get { return picOneWayBindingInstance.stack.stackStorage[5].ToString(); }
        }

        public string stackElement6
        {
            get { return picOneWayBindingInstance.stack.stackStorage[6].ToString(); }
        }

        public string stackElement7
        {
            get { return picOneWayBindingInstance.stack.stackStorage[7].ToString(); }
        }
        #endregion
    }
}
