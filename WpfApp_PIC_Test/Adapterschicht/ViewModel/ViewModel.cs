using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfApp_PIC.Anwednungsschicht;

namespace WpfApp_PIC.Adapterschicht.ViewModel;

public class DataRegisterViewModel : ViewModelBase
{
    private readonly IDataRegisterService _dataRegisterService;

    // Eingabewerte
    private int _registerIndex;
    private int _registerValue;

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

    private ObservableCollection<int> _registerValues;

    public ObservableCollection<int> RegisterValues
    {
        get => _registerValues;
        set => SetProperty(ref _registerValues, value);
    }

    public ICommand UpdateRegisterCommand { get; }

    public DataRegisterViewModel(IDataRegisterService dataRegisterService)
    {
        _dataRegisterService = dataRegisterService;
        LoadRegisterValues();

        // Befehl zum Übermitteln der Änderungen ohne Argument
        UpdateRegisterCommand = new RelayCommand(UpdateRegister);
    }


    private void LoadRegisterValues()
    {
        var values = _dataRegisterService.GetRegisterValues();
        RegisterValues = new ObservableCollection<int>(values);
    }

    private void UpdateRegister()
    {
        // Wenn der Text in RegisterValue ungültig ist, auf 0 setzen
        if (string.IsNullOrWhiteSpace(RegisterValue.ToString()))
        {
            RegisterValue = 0; // Wenn leer, auf 0 setzen
        }

        // Prüfen, ob der Index im gültigen Bereich liegt
        if (RegisterIndex < 0 || RegisterIndex >= RegisterValues.Count)
        {
            // Optional: Fehlermeldung oder Validierung
            return;
        }

        // Den Wert im Register auf den angegebenen Wert setzen
        _dataRegisterService.SetValue(RegisterIndex, RegisterValue);

        // Die Registerwerte neu laden, um die GUI zu aktualisieren
        LoadRegisterValues();
    }



}