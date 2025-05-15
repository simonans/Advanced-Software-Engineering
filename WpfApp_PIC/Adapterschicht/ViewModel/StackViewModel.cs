using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp_PIC.Anwednungsschicht;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;

namespace WpfApp_PIC.Adapterschicht.ViewModel
{
    public class StackViewModel : ViewModelBase
    {
        private readonly StackService _stackService;
        private ObservableCollection<int> _stackValues;

        public ObservableCollection<int> StackValues
        {
            get => _stackValues;
            private set => SetProperty(ref _stackValues, value);
        }

        public StackViewModel(StackService stackService)
        {
            _stackService = stackService;
            LoadStackValues();

            // Event abonnieren
            _stackService.StatusChanged += (sender, args) =>
            {
                LoadStackValues();
                OnPropertyChanged(nameof(StackValues));
            };
        }

        private void LoadStackValues()
        {
            StackValues = new ObservableCollection<int>(_stackService.GetStackValues());
        }
    }
}
