using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DataRegisterServices
{
    public class RegularSFRService : ISFRService
    {
        private DataRegister _dataRegister;
        private int _address;


        public event EventHandler ValueChanged;

        public RegularSFRService(DataRegister dataRegister, int address) 
        { 
            _dataRegister = dataRegister;
            _address = address;
        }

        public DataRegister DataRegister { get { return _dataRegister; } }

        public int Address { get { return _address; } }

        public void SetValue(int Value)
        {
            _dataRegister.SetValue(_address, Value);
            OnValueChanged();
        }

        public int GetValue()
        {
            return _dataRegister.GetValueBank0(_address);
        }


        public virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
