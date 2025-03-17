using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Anwednungsschicht;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;

namespace WpfApp_PIC.Adapterschicht.ViewModel
{
    public class ProgramCounterViewModel : ViewModelBase
    {
        private readonly ProgramCounterService _programCounterService;

        public ProgramCounterViewModel(ProgramCounterService programCounterService)
        {
            _programCounterService = programCounterService;
            _programCounterService.ValueChanged += (sender, args) => OnPropertyChanged(nameof(PCValue));
        }
        public int PCValue
        {
            get
            {
                return _programCounterService.GetPC();
            }
            set
            {
                if (_programCounterService.GetPC() != value)
                {
                    _programCounterService.SetPC(value);
                    OnPropertyChanged(nameof(PCValue));
                }
            }
        }
    }
}
