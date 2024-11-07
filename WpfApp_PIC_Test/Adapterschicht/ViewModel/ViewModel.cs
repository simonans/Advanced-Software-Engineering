using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfApp_PIC.Anwednungsschicht;

namespace WpfApp_PIC.Adapterschicht.ViewModel;

public class DataRegisterViewModel : ViewModelBase
{
    private readonly IDataRegisterService _dataRegisterService;
    private ObservableCollection<int> _registerValues;

    public ObservableCollection<int> RegisterValues
    {
        get => _registerValues;
        set => SetProperty(ref _registerValues, value);
    }

    public ICommand UpdateValueCommand { get; }

    public DataRegisterViewModel(IDataRegisterService dataRegisterService)
    {
        _dataRegisterService = dataRegisterService;
        LoadRegisterValues();

        // Verwenden von RelayCommand mit string als Parameter
        UpdateValueCommand = new RelayCommand<string>(UpdateValue);
    }

    private void LoadRegisterValues()
    {
        var values = _dataRegisterService.GetRegisterValues();
        RegisterValues = new ObservableCollection<int>(values);
    }

    private void UpdateValue(string valuesString)
    {
        // Prüfen, ob der Eingabewert leer ist
        if (string.IsNullOrWhiteSpace(valuesString)) return;

        // Konvertiere den Eingabestring in ein Integer-Array
        var values = valuesString.Split(',')
                                 .Select(v => int.TryParse(v, out int result) ? result : 0)
                                 .ToArray();

        for (int i = 0; i < values.Length; i++)
        {
            if (i < RegisterValues.Count) // Vermeiden von Array-Index-Fehlern
            {
                _dataRegisterService.SetValue(i, values[i]);
            }
        }

        // Aktuelles Register nach der Aktualisierung neu laden
        LoadRegisterValues();
    }
}
