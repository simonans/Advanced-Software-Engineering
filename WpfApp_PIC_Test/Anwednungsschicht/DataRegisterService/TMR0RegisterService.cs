using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DatenspeicherService
{
    class TMR0RegisterService 
    {
        private DataRegister _dataRegister;

        public TMR0RegisterService(DataRegister dataRegister)
        {
            _dataRegister = dataRegister;
        }

        public int GetValue()
        {
            return _dataRegister.GetValue(1);
        }

        public void SetValue(int value)
        {
            _dataRegister.SetValue(1, value);
        }

        public void IncreaseValue()
        {
            _dataRegister.SetValue(1, GetValue() + 1);
        }
    }
}
