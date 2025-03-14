using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Adapterschicht.ViewModel
{
    public  class MainViewModel : ViewModelBase
    {
        public DataRegisterViewModel dataRegisterViewModel { get; }
        public MainViewModel(DataRegisterViewModel dataRegisterViewModel)
        {
            this.dataRegisterViewModel = dataRegisterViewModel;
        }
    }
}
