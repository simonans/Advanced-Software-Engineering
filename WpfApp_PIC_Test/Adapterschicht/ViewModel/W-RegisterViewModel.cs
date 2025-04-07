using System;
using System.ComponentModel;
using WpfApp_PIC.Anwednungsschicht;

namespace WpfApp_PIC.Adapterschicht.ViewModel
{
    public class W_RegisterViewModel : ViewModelBase
    {
        private readonly W_RegisterService _w_RegisterService;
        public W_RegisterViewModel(W_RegisterService w_RegisterService)
        {
            _w_RegisterService = w_RegisterService;
            _w_RegisterService.ValueChanged += (sender, args) => OnPropertyChanged(nameof(WValue));
        }

        public int WValue
        {
            get => _w_RegisterService.GetValue();
            set
            {
                if (_w_RegisterService.GetValue() != value)
                {
                    _w_RegisterService.SetValue(value);
                    OnPropertyChanged(nameof(WValue));
                }
            }
        }
    }
}