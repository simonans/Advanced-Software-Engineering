using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfApp_PIC.Anwednungsschicht;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;
using WpfApp_PIC.Domänenschicht;
using WpfApp_PIC.Anwednungsschicht.DataRegisterServices;
using System.ComponentModel;

namespace WpfApp_PIC.Adapterschicht.ViewModel;

public class DataRegisterViewModel : ViewModelBase
{
    private readonly DataRegisterService _dataRegisterService;
    private readonly RegularSFRService _statusRegisterService;
    private readonly RegularSFRService _pclathRegisterService;
    private readonly RegularSFRService _pclRegisterService;
    private readonly RegularSFRService _tmr0registerservice;
    private readonly Bank1DecoratorService _portAService;
    private readonly Bank1DecoratorService _portBService;

    #region Datenspeicher Bank0 und Bank1
    private int _registerIndex;
    private int _registerValue;

    private int _bank1Value;

    public int RegisterIndex
    {
        get => _registerIndex;
        set => SetProperty(ref _registerIndex, value);
    }

    public int RegisterValue
    {
        get => _registerValue;
        set => SetProperty(ref _registerValue, value);
    }

    public int Bank1Value
    {
        get => _bank1Value;
        set => SetProperty(ref _bank1Value, value);
    }

    private ObservableCollection<int> _registerValues;

    private ObservableCollection<int> _bank1Values;


    public ObservableCollection<int> RegisterValues
    {
        get => _registerValues;
        set => SetProperty(ref _registerValues, value);
    }

    public ObservableCollection<int> Bank1Values
    {
        get => _bank1Values;
        set => SetProperty(ref _bank1Values, value);
    }

    public ICommand UpdateRegisterCommand { get; }

    private void LoadRegisterValues()
    {
        var registerValues = _dataRegisterService.GetAllBank0Values();
        RegisterValues = new ObservableCollection<int>(registerValues);
    }

    private void LoadBank1Values()
    {
        var bank1Values = _dataRegisterService.GetAllBank1Values();
        Bank1Values = new ObservableCollection<int>(bank1Values);
    }

    private void UpdateRegister()
    {
        // Den Wert im Register auf den angegebenen Wert setzen
        _dataRegisterService.SetValue(RegisterIndex, RegisterValue);

        // Die Registerwerte neu laden, um die GUI zu aktualisieren
        LoadRegisterValues();
        LoadBank1Values();
    }
    #endregion

    public DataRegisterViewModel(DataRegisterService dataRegisterService, RegularSFRService statusRegisterService, RegularSFRService pclathRegisterService, RegularSFRService pclRegisterService, RegularSFRService tmr0RegisterService, RegularSFRService portAServiceBase, RegularSFRService portBServiceBase)
    {
        _dataRegisterService = dataRegisterService;
        _statusRegisterService = statusRegisterService;
        _pclathRegisterService = pclathRegisterService;
        _pclRegisterService = pclRegisterService;
        _tmr0registerservice = tmr0RegisterService;
        _portAService = new Bank1DecoratorService(portAServiceBase);
        _portBService = new Bank1DecoratorService(portBServiceBase);

        LoadRegisterValues();
        LoadBank1Values();

        // Befehl zum Übermitteln der Änderungen ohne Argument
        UpdateRegisterCommand = new RelayCommand(UpdateRegister);

        // Event abonnieren
        #region Status Register (Manipulation von RP1-Bit_6 und IRP-Bit_7 müssen nicht iplemntiert werden, da nur zwei Bänkeim Datenspeicher vorhanden)
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(CarryFlagBitValue));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(DCFlagBitValue));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(ZeroFlagBitValue));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(PDFlagBitValue));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(TOFlagBitValue));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(RP0BitValue));
        #endregion
 
        #region Um die GUI zu aktualisieren, wenn sich die Registerwerte ändern
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(PCLATHRegisterValue));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(PCLRegisterValue));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(TMR0RegisterValue));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(PortRA));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(PortRB));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(TrisRA));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(TrisRB));
        #endregion

        #region SFRs (Special Function Registers) aktualisieren
        _pclathRegisterService.ValueChanged += (sender, args) => OnPropertyChanged(nameof(PCLATHRegisterValue));
        _pclRegisterService.ValueChanged += (sender, args) => OnPropertyChanged(nameof(PCLRegisterValue));
        _tmr0registerservice.ValueChanged += (sender, args) => OnPropertyChanged(nameof(TMR0RegisterValue));
        _portAService.ValueChanged += (sender, args) => OnPropertyChanged(nameof(PortRA));
        _portBService.ValueChanged += (sender, args) => OnPropertyChanged(nameof(PortRB));
        _portAService.ValueChanged += (sender, args) => OnPropertyChanged(nameof(TrisRA));
        _portBService.ValueChanged += (sender, args) => OnPropertyChanged(nameof(TrisRB));
        #endregion



    }

    #region Status Register (Manipulation von RP1-Bit_6 und IRP-Bit_7 müssen nicht iplemntiert werden, da nur zwei Bänkeim Datenspeicher vorhanden)

    public int CarryFlagBitValue
    {
        get
        {
            return _statusRegisterService.GetBit(0);
        }
        set
        {
            if (_statusRegisterService.GetBit(0) != value)
            {
                _statusRegisterService.SetBit(0);
                OnPropertyChanged(nameof(CarryFlagBitValue));
            }
        }
    }

    public int DCFlagBitValue
    {
        get
        {
            return _statusRegisterService.GetBit(1);
        }
        set
        {
            if (_statusRegisterService.GetBit(1) != value)
            {
                _statusRegisterService.SetBit(1);
                OnPropertyChanged(nameof(DCFlagBitValue));
            }
        }
    }

    public int ZeroFlagBitValue
    {
        get
        {
            return _statusRegisterService.GetBit(2);
        }
        set
        {
            if (_statusRegisterService.GetBit(2) != value)
            {
                _statusRegisterService.SetBit(2);
                OnPropertyChanged(nameof(ZeroFlagBitValue));
            }
        }
    }

    public int PDFlagBitValue
    {
        get
        {
            return _statusRegisterService.GetBit(3);
        }
        set
        {
            if (_statusRegisterService.GetBit(3) != value)
            {
                _statusRegisterService.SetBit(3);
                OnPropertyChanged(nameof(PDFlagBitValue));
            }
        }
    }

    public int TOFlagBitValue
    {
        get
        {
            return _statusRegisterService.GetBit(4);
        }
        set
        {
            if (_statusRegisterService.GetBit(4) != value)
            {
                _statusRegisterService.SetBit(4);
                OnPropertyChanged(nameof(TOFlagBitValue));
            }
        }
    }

    public int RP0BitValue
    {
        get
        {
            return _statusRegisterService.GetBit(5);
        }
        set
        {
            if (_statusRegisterService.GetBit(5) != value)
            {
                _statusRegisterService.SetBit(5);
                OnPropertyChanged(nameof(RP0BitValue));
            }
        }
    }
    #endregion

    public int PCLATHRegisterValue
    {
        get
        {
            return _pclathRegisterService.GetValue();
        }
        set
        {
            if (_pclathRegisterService.GetValue() != value)
            {
                _pclathRegisterService.SetValue(value);
                OnPropertyChanged(nameof(PCLATHRegisterValue));
            }
        }
    }

    public int PCLRegisterValue
    {
        get
        {
            return _pclRegisterService.GetValue();
        }
        set
        {
            if (_pclRegisterService.GetValue() != value)
            {
                _pclRegisterService.SetValue(value);
                OnPropertyChanged(nameof(PCLRegisterValue));
            }
        }
    }

    public int TMR0RegisterValue
    {
        get
        {
            return _tmr0registerservice.GetValue();
        }
        set
        {
            if (_tmr0registerservice.GetValue() != value)
            {
                _tmr0registerservice.SetValue(value);
                OnPropertyChanged(nameof(TMR0RegisterValue));
            }
        }
    }

    public int PortRA
    {
        get
        {
            return _portAService.GetValue();
        }
        set
        {
            if (_portAService.GetValue() != value)
            {
                _portAService.SetValue(value);
                OnPropertyChanged(nameof(PortRA));
            }
        }
    }
    public int PortRB
    {
        get
        {
            return _portBService.GetValue();
        }
        set
        {
            if (_portBService.GetValue() != value)
            {
                _portBService.SetValue(value);
                OnPropertyChanged(nameof(PortRB));
            }
        }
    }
    public int TrisRA
    {
        get
        {
            return _portAService.GetValueTris();
        }
        set
        {
            if (_portAService.GetValueTris() != value)
            {
                _portAService.SetValue(value);
                OnPropertyChanged(nameof(TrisRA));
            }
        }
    }
    public int TrisRB
    {
        get
        {
            return _portBService.GetValueTris();
        }
        set
        {
            if (_portBService.GetValueTris() != value)
            {
                _portBService.SetValue(value);
                OnPropertyChanged(nameof(TrisRB));
            }
        }
    }

}