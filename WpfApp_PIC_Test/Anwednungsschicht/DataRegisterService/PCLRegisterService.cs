using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DatenspeicherService
{
    public class PCLRegisterService 
    {
        private DataRegister _dataRegister;

        public PCLRegisterService(DataRegister dataRegister)
        {
            _dataRegister = dataRegister;
        }

        public int GetValue()
        {
            return _dataRegister.GetValue(0);
        }

        public void SetValue(int value)
        {
            _dataRegister.SetValue(2, value);
        }

        public void increaseValue()
        {
            _dataRegister.SetValue(2, GetValue() + 1);
        }
    }
}
