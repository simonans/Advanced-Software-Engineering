using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfApp_PIC.Anwednungsschicht;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Adapterschicht.ViewModel;

public class DataRegisterViewModel : ViewModelBase
{
    private readonly DataRegisterService _dataRegisterService;
    private readonly StatusRegisterService _statusRegisterService;
    private readonly PCLATHRegisterService _pclathRegisterService;
    private readonly PCLRegisterService _pclRegisterService;

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

    public DataRegisterViewModel(DataRegisterService dataRegisterService, StatusRegisterService statusRegisterService, PCLATHRegisterService pclathRegisterService, PCLRegisterService pclRegisterService)
    {
        _dataRegisterService = dataRegisterService;
        _statusRegisterService = statusRegisterService;
        _pclathRegisterService = pclathRegisterService;
        _pclRegisterService = pclRegisterService;

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
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(PCLATHRegisterValue));
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(PCLRegisterValue));



    }

    #region Status Register (Manipulation von RP1-Bit_6 und IRP-Bit_7 müssen nicht iplemntiert werden, da nur zwei Bänkeim Datenspeicher vorhanden)

    public int CarryFlagBitValue
    {
        get
        {
            return _statusRegisterService.GetCarryFlag();
        }
        set
        {
            if (_statusRegisterService.GetCarryFlag() != value)
            {
                _statusRegisterService.SetCarryFlag();
                OnPropertyChanged(nameof(CarryFlagBitValue));
            }
        }
    }

    public int DCFlagBitValue
    {
        get
        {
            return _statusRegisterService.GetDCFlag();
        }
        set
        {
            if (_statusRegisterService.GetDCFlag() != value)
            {
                _statusRegisterService.SetDCFlag();
                OnPropertyChanged(nameof(DCFlagBitValue));
            }
        }
    }

    public int ZeroFlagBitValue
    {
        get
        {
            return _statusRegisterService.GetZeroFlag();
        }
        set
        {
            if (_statusRegisterService.GetZeroFlag() != value)
            {
                _statusRegisterService.SetZeroFlag();
                OnPropertyChanged(nameof(ZeroFlagBitValue));
            }
        }
    }

    public int PDFlagBitValue
    {
        get
        {
            return _statusRegisterService.GetPDFlag();
        }
        set
        {
            if (_statusRegisterService.GetPDFlag() != value)
            {
                _statusRegisterService.SetPDFlag();
                OnPropertyChanged(nameof(PDFlagBitValue));
            }
        }
    }

    public int TOFlagBitValue
    {
        get
        {
            return _statusRegisterService.GetTOFlag();
        }
        set
        {
            if (_statusRegisterService.GetTOFlag() != value)
            {
                _statusRegisterService.SetTOFlag();
                OnPropertyChanged(nameof(TOFlagBitValue));
            }
        }
    }

    public int RP0BitValue
    {
        get
        {
            return _statusRegisterService.GetRP0();
        }
        set
        {
            if (_statusRegisterService.GetRP0() != value)
            {
                _statusRegisterService.SetRP0();
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


}