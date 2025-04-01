using System;
using System.ComponentModel;
using WpfApp_PIC.Anwednungsschicht;

namespace WpfApp_PIC.Adapterschicht.ViewModel
{
    public class W_RegisterViewModel
    {
        private readonly W_RegisterService _w_RegisterService;
        private readonly PropertyChangedNotifier _notifier = new PropertyChangedNotifier();

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _notifier.PropertyChanged += value; }
            remove { _notifier.PropertyChanged -= value; }
        }

        public W_RegisterViewModel(W_RegisterService w_RegisterService)
        {
            _w_RegisterService = w_RegisterService;
            _w_RegisterService.ValueChanged += (sender, args) => _notifier.OnPropertyChanged(nameof(WValue));
        }

        public int WValue
        {
            get => _w_RegisterService.GetValue();
            set
            {
                if (_w_RegisterService.GetValue() != value)
                {
                    _w_RegisterService.SetValue(value);
                    _notifier.OnPropertyChanged(nameof(WValue));
                }
            }
        }
    }
}