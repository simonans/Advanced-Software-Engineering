using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DatenspeicherService
{
    public class PortService
    {
        private readonly DataRegister _dataRegister;
        public event EventHandler ValueChanged;

        public PortService(DataRegister dataRegister)
        {
            _dataRegister = dataRegister;
        }

        public int GetValuePortRA()
        {
            return _dataRegister.GetValue(5);
        }
        public int GetValuePortBB()
        {
            return _dataRegister.GetValue(6);
        }
        public int GetValueTrisRA()
        {
            return _dataRegister.GetValueBank1(5);
        }
        public int GetValueTrisRB()
        {
            return _dataRegister.GetValueBank1(6);
        }
        public void SetValuePort(int Value, int Port)
        {
            if (Port == 5)
            {
                _dataRegister.SetValue(5, Value);
            }
            else if (Port == 6)
            {
                _dataRegister.SetValue(6, Value);
            }
            OnValueChanged();
        }

        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
