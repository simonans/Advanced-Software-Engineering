using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfApp_PIC.Anwednungsschicht;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService.StatusRegisterService;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Adapterschicht.ViewModel;

public class DataRegisterViewModel : ViewModelBase
{
    private readonly IDataRegisterService _dataRegisterService;

    #region Datenspeicher Bank0 und Bank1
    private int _registerIndex;
    private int _registerValue;

    private int _bank1Index;
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

    public int Bank1Index
    {
        get => _bank1Index;
        set => SetProperty(ref _bank1Index, value);
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

    public ICommand UpdateBank1Command { get; }

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

    private void UpdateBank1()
    {
        // Den Wert im Register auf den angegebenen Wert setzen
        _dataRegisterService.SetValue(Bank1Index, Bank1Value);

        // Die Registerwerte neu laden, um die GUI zu aktualisieren
        LoadBank1Values();
        LoadRegisterValues();
    }
    #endregion

    public DataRegisterViewModel(IDataRegisterService dataRegisterService, IStatusRegisterService statusRegisterService)
    {
        _dataRegisterService = dataRegisterService;
        _statusRegisterService = statusRegisterService;

        LoadRegisterValues();
        LoadBank1Values();

        // Befehl zum Übermitteln der Änderungen ohne Argument
        UpdateRegisterCommand = new RelayCommand(UpdateRegister);
        UpdateBank1Command = new RelayCommand(UpdateBank1);

        // Event abonnieren
        _dataRegisterService.StatusChanged += (sender, args) => OnPropertyChanged(nameof(RP0BitValue));

    }

    #region Status Register (bisher nur RP0-Bit)
    private readonly IStatusRegisterService _statusRegisterService;

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
                _statusRegisterService.SetRP0(value);
                OnPropertyChanged(nameof(RP0BitValue));
            }
        }
    }
    #endregion
}