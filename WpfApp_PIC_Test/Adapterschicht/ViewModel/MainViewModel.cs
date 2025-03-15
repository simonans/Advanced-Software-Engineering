using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Anwednungsschicht;

namespace WpfApp_PIC.Adapterschicht.ViewModel
{
    public  class MainViewModel : ViewModelBase
    {
        public DataRegisterViewModel dataRegisterViewModel { get; }
        public StackViewModel stackViewModel { get; }
        public ProgramCounterViewModel programCounterViewModel { get; }
        public W_RegisterViewModel w_RegisterViewModel{ get; }
        public MainViewModel(DataRegisterViewModel dataRegisterViewModel, StackViewModel stackViewModel, ProgramCounterViewModel programMemoryViewModel, W_RegisterViewModel w_RegisterViewModel)
        {
            this.dataRegisterViewModel = dataRegisterViewModel;
            this.stackViewModel = stackViewModel;
            this.programCounterViewModel = programMemoryViewModel;
            this.w_RegisterViewModel = w_RegisterViewModel;
        }
    }
}
